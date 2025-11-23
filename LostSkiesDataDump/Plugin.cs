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

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Bossa.Dynamika.Utilities;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using LostSkiesDataDump.Converters.Bossa.Dynamika.Utilities;
using LostSkiesDataDump.Converters.Global;
using LostSkiesDataDump.Converters.Il2CppSystem.Collections.Generic;
using LostSkiesDataDump.Converters.UnityEngine;
using LostSkiesDataDump.Converters.UnityEngine.Localization;
using LostSkiesDataDump.Converters.Utilities.Weapons;
using LostSkiesDataDump.Converters.WildSkies.Gameplay.Crafting;
using LostSkiesDataDump.Converters.WildSkies.Gameplay.Items;
using LostSkiesDataDump.Converters.WildSkies.Service;
using LostSkiesDataDump.Converters.WildSkies.Weapon;
using LostSkiesDataDump.Converters.WildSkies.WorldItems;
using UnityEngine;
using UnityEngine.Localization;
using Utilities.Weapons;
using WildSkies.Gameplay.Crafting;
using WildSkies.Gameplay.Items;
using WildSkies.Service;
using WildSkies.Weapon;
using WildSkies.WorldItems;

namespace LostSkiesDataDump;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    private static ManualLogSource s_log = null;
    internal static new ManualLogSource Log
    {
        get =>
            s_log
            ?? throw new InvalidOperationException(
                $"{MyPluginInfo.PLUGIN_GUID} not yet initialized"
            );
        private set => s_log = value;
    }

    private static ConfigEntry<string> s_configBaseOutputDirectory = null;
    protected static ConfigEntry<string> ConfigBaseOutputDirectory
    {
        get =>
            s_configBaseOutputDirectory
            ?? throw new InvalidOperationException(
                $"{MyPluginInfo.PLUGIN_GUID} not yet initialized"
            );
        private set => s_configBaseOutputDirectory = value;
    }
    public static string BaseOutputDirectory => ConfigBaseOutputDirectory.Value;

    private static ConfigEntry<string> s_configIconOutputDirectory = null;
    protected static ConfigEntry<string> ConfigIconOutputDirectory
    {
        get =>
            s_configIconOutputDirectory
            ?? throw new InvalidOperationException(
                $"{MyPluginInfo.PLUGIN_GUID} not yet initialized"
            );
        private set => s_configIconOutputDirectory = value;
    }
    public static string IconOutputDirectory =>
        Path.Join(BaseOutputDirectory, ConfigIconOutputDirectory.Value);

    private static ConfigEntry<string> s_configTextOutputFile = null;
    protected static ConfigEntry<string> ConfigTextOutputFile
    {
        get =>
            s_configTextOutputFile
            ?? throw new InvalidOperationException(
                $"{MyPluginInfo.PLUGIN_GUID} not yet initialized"
            );
        private set => s_configTextOutputFile = value;
    }
    public static string TextOutputFile =>
        Path.Join(BaseOutputDirectory, ConfigTextOutputFile.Value);

    private static Harmony s_harmony = null;

    private static SerializationRoot s_serializationRoot = null;
    public static SerializationRoot SerializationRoot => s_serializationRoot ??= new();

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo(
            $"Loading plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION}..."
        );
        Log.LogInfo(
            $"  - config file: {Config.ConfigFilePath}{(File.Exists(Config.ConfigFilePath) ? "" : " (does not exist)")}"
        );
        ConfigBaseOutputDirectory = Config.Bind(
            "Output",
            "BaseDirectory",
            Path.Join(Application.dataPath, MyPluginInfo.PLUGIN_GUID),
            "The (base) directory that this plugin saves its output to."
        );
        Log.LogInfo($"  - base output directory: {BaseOutputDirectory}");
        ConfigIconOutputDirectory = Config.Bind(
            "Output",
            "IconDirectory",
            "icons",
            $"The directory that this plugin saves icons files to.  Its path is relative to that of the base output directory ({ConfigBaseOutputDirectory.Definition.Section}.{ConfigBaseOutputDirectory.Definition.Key})."
        );
        Log.LogInfo($"  - icon output directory: {IconOutputDirectory}");
        ConfigTextOutputFile = Config.Bind(
            "Output",
            "TextFile",
            "data.json",
            $"The file that this plugin saves text output to.  Its path is relative to that of the base output directory ({ConfigBaseOutputDirectory.Definition.Section}.{ConfigBaseOutputDirectory.Definition.Key})."
        );
        Log.LogInfo($"  - text output file: {TextOutputFile}");
        s_harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        s_harmony.PatchAll(typeof(Patch));
        Log.LogInfo(
            $"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} has loaded."
        );
    }

    public override bool Unload()
    {
        Log.LogInfo(
            $"Unloading plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION}..."
        );
        s_harmony?.UnpatchSelf();
        Log.LogInfo(
            $"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} has unloaded."
        );
        return base.Unload();
    }

    public static void DumpData()
    {
        Log.LogInfo("Dumping data");
        try
        {
            Log.LogInfo(Directory.CreateDirectory(BaseOutputDirectory));
            using FileStream textOutputStream = File.Create(TextOutputFile);
            var referenceHandler = new DataReferenceHandler();
            JsonSerializerOptions jsonSerializerOptions = new()
            {
                WriteIndented = true,
                ReferenceHandler = referenceHandler,
            };
            AddConverters(jsonSerializerOptions);
            try
            {
                JsonSerializer.Serialize(
                    textOutputStream,
                    SerializationRoot,
                    jsonSerializerOptions
                );
            }
            finally
            {
                // Reset after serializing to avoid out of bounds memory growth in the resolver.
                referenceHandler.Reset();
            }
        }
        catch (Exception e)
        {
            Log.LogError("Error during data dump");
            Log.LogError(e);
        }
        finally
        {
            Log.LogInfo("Data dump complete");
        }
    }

    public static void AddConverters(JsonSerializerOptions options)
    {
        // Compendium
        options.Converters.Add(new ICompendiumServiceConverter<ICompendiumService>());
        options.Converters.Add(new CompendiumCategoryConverter<CompendiumCategory>());
        options.Converters.Add(new CompendiumEntryConverter<CompendiumEntry>());
        // WildSkies.Gameplay.Crafting
        options.Converters.Add(new CraftingComponentConverter<CraftingComponent>());
        options.Converters.Add(new CraftableItemBlueprintConverter<CraftableItemBlueprint>());
        // WildSkies.Service
        options.Converters.Add(new ICraftingServiceConverter<ICraftingService>());
        options.Converters.Add(new WorldRegionServiceConverter<WorldRegionService>());
        // WildSkies.WorldItems
        options.Converters.Add(new RegionIdentifierDataConverter<RegionIdentifierData>());
        // Crafting
        options.Converters.Add(new RandomStatsDefinitionConverter<RandomStatsDefinition>());
        // Items
        options.Converters.Add(new IItemServiceConverter<IItemService>());
        options.Converters.Add(new ItemDefinitionConverter<ItemDefinition>());
        options.Converters.Add(new ItemDefinitionConverter<ItemDefinition>());
        // Items: Item Components
        options.Converters.Add(new ItemAccessoryComponentConverter<ItemAccessoryComponent>());
        options.Converters.Add(new ItemAmmoComponentConverter<ItemAmmoComponent>());
        options.Converters.Add(
            new ItemCustomisationComponentConverter<ItemCustomisationComponent>()
        );
        options.Converters.Add(new ItemKnowledgeComponentConverter<ItemKnowledgeComponent>());
        options.Converters.Add(new ItemThrowableComponentConverter<ItemThrowableComponent>());
        options.Converters.Add(new ItemWeaponComponentConverter<ItemWeaponComponent>());
        options.Converters.Add(new ItemWorldComponentConverter<ItemWorldComponent>());
        options.Converters.Add(new BaseItemComponentConverter<BaseItemComponent>());
        // Item: Profile
        options.Converters.Add(new WeaponProfileConverter<WeaponProfile>());
        options.Converters.Add(new ItemProfileConverter<ItemProfile>());
        // Item: Level
        options.Converters.Add(new WeaponLevelsConverter<WeaponLevels>());
        options.Converters.Add(new ItemLevelConverter<ItemLevel>());
        options.Converters.Add(new ItemLevelsConverter<ItemLevels>());
        // Item: Utility
        options.Converters.Add(new WeaponBaseConverter<WeaponBase>());
        options.Converters.Add(new UtilityItemConverter<UtilityItem>());
        // Miscellaneous
        options.Converters.Add(new LocalizedStringConverter<LocalizedString>());
        options.Converters.Add(new QuaternionConverter());
        options.Converters.Add(new Vector2Converter());
        options.Converters.Add(new Vector2IntConverter());
        options.Converters.Add(new Vector3Converter());
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new DictionaryConverterFactory());
        options.Converters.Add(new ListConverterFactory());
    }
}
