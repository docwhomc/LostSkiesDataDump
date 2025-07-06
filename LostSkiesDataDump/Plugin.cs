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
    internal static new ManualLogSource Log;
    private static ConfigEntry<string> s_configBaseOutputDirectory;
    private static ConfigEntry<string> s_configIconOutputDirectory;
    private static ConfigEntry<string> s_configTextOutputFile;
    private static Harmony s_harmony;
    private static SerializationRoot s_serializationRoot = null;

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo($"Loading plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION}...");
        Log.LogInfo($"  - config file: {Config.ConfigFilePath}{(File.Exists(Config.ConfigFilePath) ? "" : " (does not exist)")}");
        s_configBaseOutputDirectory = Config.Bind(
            "Output",
            "BaseDirectory",
            Path.Join(Application.dataPath, MyPluginInfo.PLUGIN_GUID),
            "The (base) directory that this plugin saves its output to."
        );
        Log.LogInfo($"  - base output directory: {BaseOutputDirectory}");
        s_configIconOutputDirectory = Config.Bind(
            "Output",
            "IconDirectory",
            "icons",
            $"The directory that this plugin saves icons files to.  Its path is relative to that of the base output directory ({s_configBaseOutputDirectory.Definition.Section}.{s_configBaseOutputDirectory.Definition.Key})."
        );
        Log.LogInfo($"  - icon output directory: {IconOutputDirectory}");
        s_configTextOutputFile = Config.Bind(
            "Output",
            "TextFile",
            "data.json",
            $"The file that this plugin saves text output to.  Its path is relative to that of the base output directory ({s_configBaseOutputDirectory.Definition.Section}.{s_configBaseOutputDirectory.Definition.Key})."
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

    public static SerializationRoot SerializationRoot
    {
        get
        {
            s_serializationRoot ??= new();
            return s_serializationRoot;
        }
    }

    public static string BaseOutputDirectory => s_configBaseOutputDirectory.Value;
    public static string IconOutputDirectory => Path.Join(BaseOutputDirectory, s_configIconOutputDirectory.Value);
    public static string TextOutputFile => Path.Join(BaseOutputDirectory, s_configTextOutputFile.Value);

    public static void DumpData()
    {
        Log.LogInfo("Dumping data");
        Log.LogInfo(Directory.CreateDirectory(BaseOutputDirectory));
        using FileStream textOutputStream = File.Create(TextOutputFile);
        JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };
        jsonSerializerOptions.Converters.Add(new CLocalizedString());
        jsonSerializerOptions.Converters.Add(new CICompendiumService());
        jsonSerializerOptions.Converters.Add(new CCompendiumCategory());
        jsonSerializerOptions.Converters.Add(new CCompendiumEntry());
        JsonSerializer.Serialize(textOutputStream, SerializationRoot, jsonSerializerOptions);
        Log.LogInfo("Data dump complete");
    }
}
