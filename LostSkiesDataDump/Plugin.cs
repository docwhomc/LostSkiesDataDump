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
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using LostSkiesDataDump.Converters;
using LostSkiesDataDump.Converters.Compendium;
using UnityEngine;

namespace LostSkiesDataDump;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    private static ManualLogSource s_log = null;
    internal static new ManualLogSource Log
    {
        get => s_log ?? throw new InvalidOperationException($"{MyPluginInfo.PLUGIN_GUID} not yet initialized");
        private set => s_log = value;
    }

    private static ConfigEntry<string> s_configBaseOutputDirectory = null;
    protected static ConfigEntry<string> ConfigBaseOutputDirectory
    {
        get => s_configBaseOutputDirectory ?? throw new InvalidOperationException($"{MyPluginInfo.PLUGIN_GUID} not yet initialized");
        private set => s_configBaseOutputDirectory = value;
    }
    public static string BaseOutputDirectory => ConfigBaseOutputDirectory.Value;

    private static ConfigEntry<string> s_configIconOutputDirectory = null;
    protected static ConfigEntry<string> ConfigIconOutputDirectory
    {
        get => s_configIconOutputDirectory ?? throw new InvalidOperationException($"{MyPluginInfo.PLUGIN_GUID} not yet initialized");
        private set => s_configIconOutputDirectory = value;
    }
    public static string IconOutputDirectory => Path.Join(BaseOutputDirectory, ConfigIconOutputDirectory.Value);

    private static ConfigEntry<string> s_configTextOutputFile = null;
    protected static ConfigEntry<string> ConfigTextOutputFile
    {
        get => s_configTextOutputFile ?? throw new InvalidOperationException($"{MyPluginInfo.PLUGIN_GUID} not yet initialized");
        private set => s_configTextOutputFile = value;
    }
    public static string TextOutputFile => Path.Join(BaseOutputDirectory, ConfigTextOutputFile.Value);

    private static Harmony s_harmony = null;

    private static SerializationRoot s_serializationRoot = null;
    public static SerializationRoot SerializationRoot => s_serializationRoot ??= new();

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo($"Loading plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION}...");
        Log.LogInfo($"  - config file: {Config.ConfigFilePath}{(File.Exists(Config.ConfigFilePath) ? "" : " (does not exist)")}");
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
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} has loaded.");
    }

    public override bool Unload()
    {
        Log.LogInfo($"Unloading plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION}...");
        s_harmony?.UnpatchSelf();
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} has unloaded.");
        return base.Unload();
    }

    public static void DumpData()
    {
        Log.LogInfo("Dumping data");
        Log.LogInfo(Directory.CreateDirectory(BaseOutputDirectory));
        using FileStream textOutputStream = File.Create(TextOutputFile);
        JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };
        AddConverters(jsonSerializerOptions);
        JsonSerializer.Serialize(textOutputStream, SerializationRoot, jsonSerializerOptions);
        Log.LogInfo("Data dump complete");
    }

    public static void AddConverters(JsonSerializerOptions options)
    {
        options.Converters.Add(new CLocalizedString());
        options.Converters.Add(new CICompendiumService());
        options.Converters.Add(new CCompendiumCategory());
        options.Converters.Add(new CCompendiumEntry());
    }
}
