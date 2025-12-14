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
using System.Runtime.Versioning;
using HarmonyLib;
using UISystem;
using WildSkies.Mediators;
using WildSkies.Service;
using WildSkies.WorldItems;

namespace LostSkiesDataDump;

partial class Patch
{
    [HarmonyPatch(typeof(CompendiumUiMediator), nameof(CompendiumUiMediator.Initialise))]
    [HarmonyPostfix]
    [RequiresPreviewFeatures]
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
        Plugin.Log.LogDebug($"{nameof(playerGuideService)}: {playerGuideService}");
        Plugin.Log.LogDebug($"{nameof(playerInventoryService)}: {playerInventoryService}");
        Plugin.Log.LogDebug($"{nameof(craftingService)}: {craftingService}");
        Plugin.SerializationRoot.CraftingService = craftingService;
    }

    [HarmonyPatch(typeof(CompendiumService), nameof(CompendiumService.Initialise))]
    [HarmonyPostfix]
    [RequiresPreviewFeatures]
    public static void CompendiumService_Initialise(CompendiumService __instance, int __result)
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(CompendiumService_Initialise)}(...)");
        Plugin.Log.LogDebug($"{nameof(__instance)}: {__instance}");
        Plugin.Log.LogDebug($"{nameof(__result)}: {__result}");
        Plugin.SerializationRoot.CompendiumService = __instance;
    }

    [HarmonyPatch(typeof(ContainerService), nameof(ContainerService.Initialise))]
    [HarmonyPostfix]
    [RequiresPreviewFeatures]
    public static void ContainerService_Initialise(ContainerService __instance, int __result)
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(ContainerService_Initialise)}(...)");
        Plugin.Log.LogDebug($"{nameof(__instance)}: {__instance}");
        Plugin.Log.LogDebug($"{nameof(__result)}: {__result}");
        Plugin.SerializationRoot.ContainerService = __instance;
    }

    [HarmonyPatch(typeof(ItemInventoryMediator), nameof(ItemInventoryMediator.Initialise))]
    [HarmonyPostfix]
    [RequiresPreviewFeatures]
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

    [HarmonyPatch(typeof(ItemService), nameof(ItemService.Initialise))]
    [HarmonyPostfix]
    [RequiresPreviewFeatures]
    public static void ItemService_Initialise(ItemInventoryMediator __instance, int __result)
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(ItemService_Initialise)}(...)");
        Plugin.Log.LogDebug($"{nameof(__instance)}: {__instance}");
        Plugin.Log.LogDebug($"{nameof(__result)}: {__result}");
    }

    [HarmonyPatch(typeof(LootPoolService), nameof(LootPoolService.Initialise))]
    [HarmonyPostfix]
    [RequiresPreviewFeatures]
    public static void LootPoolService_Initialise(LootPoolService __instance, int __result)
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(LootPoolService_Initialise)}(...)");
        Plugin.Log.LogDebug($"{nameof(__instance)}: {__instance}");
        Plugin.SerializationRoot.LootPoolService = __instance;
        Plugin.Log.LogDebug($"{nameof(__result)}: {__result}");
    }

    [HarmonyPatch(typeof(LootTableService), nameof(LootTableService.Initialise))]
    [HarmonyPostfix]
    [RequiresPreviewFeatures]
    public static void LootTableService_Initialise(LootTableService __instance, int __result)
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(LootTableService_Initialise)}(...)");
        Plugin.Log.LogDebug($"{nameof(__instance)}: {__instance}");
        Plugin.SerializationRoot.LootTableService = __instance;
        Plugin.Log.LogDebug($"{nameof(__result)}: {__result}");
    }

    [HarmonyPatch(typeof(SteamPlatform), nameof(SteamPlatform.Initialise))]
    [HarmonyPostfix]
    [RequiresPreviewFeatures]
    public static void SteamPlatform_Initialise(SteamPlatform __instance, int __result)
    {
        Plugin.Log.LogInfo($"{nameof(Patch)}.{nameof(SteamPlatform_Initialise)}(...)");
        Plugin.Log.LogInfo($"{nameof(__instance)}: {__instance}");
        Plugin.SerializationRoot.GameVersionInfo.SteamPlatform = __instance;
        Plugin.Log.LogDebug($"{nameof(__result)}: {__result}");
        // Plugin.SerializationRoot.LogSteamPlatformData();
    }

    [HarmonyPatch(typeof(WorldRegionService), nameof(WorldRegionService.Init))]
    [HarmonyPostfix]
    [RequiresPreviewFeatures]
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
}
