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

using System.Runtime.InteropServices;
using HarmonyLib;
using UISystem;
using UnityEngine.InputSystem;
using WildSkies.Mediators;
using WildSkies.Service;

namespace LostSkiesDataDump;

class Patch
{
    [HarmonyPatch(typeof(UIInputMediator), nameof(UIInputMediator.Update))]
    [HarmonyPrefix]
    public static void UIInputMediator_Update(UIInputMediator __instance)
    {
        if (!__instance._ui.IsServiceReady || __instance._sceneService.AreWeLoadingOrInLobby() || __instance._ui.PanelManager.IsPanelShowing(UIPanelType.Popup))
        {
            return;
        }
        if (Keyboard.current.altKey.isPressed && Keyboard.current.homeKey.wasPressedThisFrame)
        {
            Plugin.Log.LogInfo("ALT + HOME");
            Plugin.DumpData();
        }
    }

    [HarmonyPatch(typeof(CompendiumUiMediator), nameof(CompendiumUiMediator.Initialise))]
    [HarmonyPrefix]
    public static void CompendiumUiMediator_Initialise(CompendiumUiMediator __instance, [DefaultParameterValue(null)] IUIService uiService, [DefaultParameterValue(null)] ICompendiumService compendiumService, [DefaultParameterValue(null)] IPlayerGuideService playerGuideService, [DefaultParameterValue(null)] IPlayerInventoryService playerInventoryService, [DefaultParameterValue(null)] ICraftingService craftingService)
    {
        Plugin.Log.LogInfo("Patch.CompendiumUiMediator_Initialise(...)");
        Plugin.Log.LogDebug($"__instance: {__instance}");
        Plugin.Log.LogDebug($"uiService: {uiService}");
        Plugin.Log.LogDebug($"compendiumService: {compendiumService}");
        Plugin.SerializationRoot.CompendiumService = compendiumService;
        Plugin.Log.LogDebug($"playerGuideService: {playerGuideService}");
        Plugin.Log.LogDebug($"playerInventoryService: {playerInventoryService}");
        Plugin.Log.LogDebug($"craftingService: {craftingService}");
    }
}
