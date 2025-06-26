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
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using LostSkiesDataDump.Data.Compendium;
using UnityEngine;
using WildSkies.Service;

namespace LostSkiesDataDump;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    private static ConfigEntry<string> configBaseOutputDirectory;
    private static ConfigEntry<string> configIconOutputDirectory;
    private static ConfigEntry<string> configTextOutputFile;
    private static Harmony harmony;
    private static Task dataDumpTask = null;
    internal static ICompendiumService CompendiumService;

    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo($"Loading plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION}...");
        Log.LogInfo($"  - config file: {Config.ConfigFilePath}{(File.Exists(Config.ConfigFilePath) ? "" : " (does not exist)")}");
        configBaseOutputDirectory = Config.Bind(
            "Output",
            "BaseDirectory",
            Path.Join(Application.dataPath, MyPluginInfo.PLUGIN_GUID),
            "The (base) directory that this plugin saves its output to."
        );
        Log.LogInfo($"  - base output directory: {BaseOutputDirectory}");
        configIconOutputDirectory = Config.Bind(
            "Output",
            "IconDirectory",
            "icons",
            $"The directory that this plugin saves icons files to.  Its path is relative to that of the base output directory ({configBaseOutputDirectory.Definition.Section}.{configBaseOutputDirectory.Definition.Key})."
        );
        Log.LogInfo($"  - icon output directory: {IconOutputDirectory}");
        configTextOutputFile = Config.Bind(
            "Output",
            "TextFile",
            "data.json",
            $"The file that this plugin saves text output to.  Its path is relative to that of the base output directory ({configBaseOutputDirectory.Definition.Section}.{configBaseOutputDirectory.Definition.Key})."
        );
        Log.LogInfo($"  - text output file: {TextOutputFile}");
        harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(Patch));
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} has loaded.");
    }

    public override bool Unload()
    {
        Log.LogInfo($"Unloading plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION}...");
        harmony?.UnpatchSelf();
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} has unloaded.");
        return base.Unload();
    }

    public static string BaseOutputDirectory
    {
        get
        {
            return configBaseOutputDirectory.Value;
        }
    }

    public static string IconOutputDirectory
    {
        get
        {
            return Path.Join(BaseOutputDirectory, configIconOutputDirectory.Value);
        }
    }

    public static string TextOutputFile
    {
        get
        {
            return Path.Join(BaseOutputDirectory, configTextOutputFile.Value);
        }
    }

    public static void StartDataDump()
    {
        if (dataDumpTask is not null)
        {
            Log.LogError($"Cannot start data dump task when one already exists: {dataDumpTask}");
            LogDataDumpStatus();
        }
        else
        {
            Log.LogInfo($"CompendiumService: {CompendiumService}");
            Log.LogInfo(SizeLine(CompendiumService.Categories, "Category", "Categories"));
            Log.LogInfo(SizeLine(CompendiumService.Entries, "Entry", "Entries"));
            dataDumpTask = Task.Run(DumpData);
            dataDumpTask.Wait();
        }
    }

    public static async Task DumpData()
    {
        Log.LogInfo("Dumping data.");
        Log.LogInfo(Directory.CreateDirectory(BaseOutputDirectory));
        using FileStream textOutputStream = File.Create(TextOutputFile);
        JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };
        if (CompendiumService is null)
        {
            Log.LogError("Cannot serialize compendium because Plugin.CompendiumSerive is null.");
        }
        else
        {
            CompendiumData compendiumData = new(CompendiumService);
            Log.LogInfo("Serializing compendium.");
            await JsonSerializer.SerializeAsync(textOutputStream, compendiumData, jsonSerializerOptions);
            Log.LogInfo("Compendium serialized.");
        }
        Log.LogInfo("Data dump complete.");
    }

    public static void LogDataDumpStatus()
    {
        Log.LogInfo($"Data Dump Task Status: {dataDumpTask?.Status}");
    }

    private static string SizeLine<T>(List<T> list, string singular, string plural)
    {
        int size = list._size;
        return $"  - {size} {(size == 1 ? singular : plural)}";
    }
}
