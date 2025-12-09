/*
  LostSkiesDataDump: A mod for outputting data from the game Lost Skies.
  Copyright (C) 2025  DocW

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System.Runtime.Versioning;
using HarmonyLib;
using UISystem;
using UnityEngine;
using UnityEngine.InputSystem;
using WildSkies.Mediators;
using WildSkies.Service;
using WildSkies.World;

namespace LostSkiesDataDump;

partial class Patch
{
    [HarmonyPatch(typeof(HostControl), nameof(HostControl.OnWorldLoadingComplete))]
    [HarmonyPostfix]
    [RequiresPreviewFeatures]
    public static void HostControl_OnWorldLoadingComplete(
        HostControl __instance,
        string worldName,
        int succesfulIslandLoads,
        int failedIslandLoads
    )
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(HostControl_OnWorldLoadingComplete)}(...)");
        Plugin.Log.LogDebug($"{nameof(__instance)}: {__instance}");
        Plugin.Log.LogDebug($"{nameof(worldName)}: {worldName}");
        Plugin.Log.LogDebug($"{nameof(succesfulIslandLoads)}: {succesfulIslandLoads}");
        Plugin.Log.LogDebug($"{nameof(failedIslandLoads)}: {failedIslandLoads}");
        Plugin.Log.LogInfo(
            $"{nameof(Application)}.{nameof(Application.isBatchMode)}: {Application.isBatchMode}"
        );
        if (Application.isBatchMode)
        {
            Plugin.DumpData();
            Plugin.Log.LogInfo($"Quitting application after data dump.");
            Application.Quit(0);
        }
    }

    [HarmonyPatch(typeof(UIInputMediator), nameof(UIInputMediator.Update))]
    [HarmonyPrefix]
    [RequiresPreviewFeatures]
    public static void UIInputMediator_Update(UIInputMediator __instance)
    {
        if (
            Application.isBatchMode
            || !__instance._ui.IsServiceReady
            || __instance._sceneService.AreWeLoadingOrInLobby()
            || __instance._ui.PanelManager.IsPanelShowing(UIPanelType.Popup)
        )
        {
            return;
        }
        if (Keyboard.current.altKey.isPressed && Keyboard.current.homeKey.wasPressedThisFrame)
        {
            Plugin.Log.LogInfo("ALT + HOME");
            Plugin.DumpData();
        }
    }

    [HarmonyPatch(typeof(SteamPlatform), nameof(SteamPlatform.OwnsApplication))]
    [HarmonyPostfix]
    public static void SteamPlatform_OwnsApplication(
        SteamPlatform __instance,
        ref bool __result,
        int targetAppID
    )
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(SteamPlatform_OwnsApplication)}(...)");
        Plugin.Log.LogInfo($"{nameof(__instance)}: {__instance}");
        Plugin.Log.LogInfo($"{nameof(__result)}: {__result}");
        Plugin.Log.LogInfo($"{nameof(targetAppID)}: {targetAppID}");
        Plugin.Log.LogInfo(
            $"{__instance}.{nameof(SteamPlatform.OwnsApplication)}({targetAppID}) => {__result}"
        );
    }

    [HarmonyPatch(typeof(SteamPlatform), nameof(SteamPlatform.HasAccessToDLC))]
    [HarmonyPostfix]
    public static void SteamPlatform_HasAccessToDLC(
        SteamPlatform __instance,
        ref bool __result,
        int dlcID
    )
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(SteamPlatform_HasAccessToDLC)}(...)");
        Plugin.Log.LogInfo($"{nameof(__instance)}: {__instance}");
        Plugin.Log.LogInfo($"{nameof(__result)}: {__result}");
        Plugin.Log.LogInfo($"{nameof(dlcID)}: {dlcID}");
        Plugin.Log.LogInfo(
            $"{__instance}.{nameof(SteamPlatform.HasAccessToDLC)}({dlcID}) => {__result}"
        );
    }
}
