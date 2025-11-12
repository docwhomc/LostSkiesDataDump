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
using WildSkies.WorldItems;

namespace LostSkiesDataDump;

class Patch
{
    [HarmonyPatch(typeof(UIInputMediator), nameof(UIInputMediator.Update))]
    [HarmonyPrefix]
    public static void UIInputMediator_Update(UIInputMediator __instance)
    {
        if (
            !__instance._ui.IsServiceReady
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

    [HarmonyPatch(typeof(CompendiumUiMediator), nameof(CompendiumUiMediator.Initialise))]
    [HarmonyPrefix]
    public static void CompendiumUiMediator_Initialise(
        CompendiumUiMediator __instance,
        [DefaultParameterValue(null)] IUIService uiService,
        [DefaultParameterValue(null)] ICompendiumService compendiumService,
        [DefaultParameterValue(null)] IPlayerGuideService playerGuideService,
        [DefaultParameterValue(null)] IPlayerInventoryService playerInventoryService,
        [DefaultParameterValue(null)] ICraftingService craftingService
    )
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(CompendiumUiMediator_Initialise)}(...)");
        Plugin.Log.LogDebug($"{nameof(__instance)}: {__instance}");
        Plugin.Log.LogDebug($"{nameof(uiService)}: {uiService}");
        Plugin.Log.LogDebug($"{nameof(compendiumService)}: {compendiumService}");
        Plugin.SerializationRoot.CompendiumService = compendiumService;
        Plugin.Log.LogDebug($"{nameof(playerGuideService)}: {playerGuideService}");
        Plugin.Log.LogDebug($"{nameof(playerInventoryService)}: {playerInventoryService}");
        Plugin.Log.LogDebug($"{nameof(craftingService)}: {craftingService}");
        Plugin.SerializationRoot.CraftingService = craftingService;
    }

    [HarmonyPatch(typeof(WorldRegionService), nameof(WorldRegionService.Init))]
    [HarmonyPrefix]
    public static void WorldRegionService_Init(
        WorldRegionService __instance,
        Map worldMap,
        WorldLoadingService worldLoadingService,
        SkyMapService skyMapService
    )
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(WorldRegionService_Init)}(...)");
        Plugin.Log.LogDebug($"{nameof(__instance)}: {__instance}");
        Plugin.SerializationRoot.WorldRegionService = __instance;
        Plugin.Log.LogDebug($"{nameof(worldMap)}: {worldMap}");
        Plugin.Log.LogDebug($"{nameof(worldLoadingService)}: {worldLoadingService}");
        Plugin.Log.LogDebug($"{nameof(skyMapService)}: {skyMapService}");
    }

    [HarmonyPatch(typeof(ItemInventoryMediator), nameof(ItemInventoryMediator.Initialise))]
    [HarmonyPrefix]
    public static void ItemInventoryMediator_Initialise(
        ItemInventoryMediator __instance,
        [DefaultParameterValue(null)] IItemService itemService,
        [DefaultParameterValue(null)] IPlayerInventoryService playerInventoryService,
        [DefaultParameterValue(null)] IUIService uiService,
        [DefaultParameterValue(null)] LocalisationService localisationService
    )
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(ItemInventoryMediator_Initialise)}(...)");
        Plugin.Log.LogDebug($"{nameof(__instance)}: {__instance}");
        Plugin.Log.LogInfo($"{nameof(itemService)}: {itemService}");
        Plugin.SerializationRoot.ItemService = itemService;
        Plugin.Log.LogInfo($"{nameof(playerInventoryService)}: {playerInventoryService}");
        Plugin.Log.LogInfo($"{nameof(uiService)}: {uiService}");
        Plugin.Log.LogInfo($"{nameof(localisationService)}: {localisationService}");
    }

    [HarmonyPatch(typeof(SteamPlatform), nameof(SteamPlatform.Initialise))]
    [HarmonyPrefix]
    public static void SteamPlatform_Initialise(SteamPlatform __instance)
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(SteamPlatform_Initialise)}(...)");
        Plugin.Log.LogInfo($"{nameof(__instance)}: {__instance}");
        Plugin.SerializationRoot.GameVersionInfo.SteamPlatform = __instance;
        // Plugin.SerializationRoot.LogSteamPlatformData();
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
