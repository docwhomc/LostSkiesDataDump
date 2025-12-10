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
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using LostSkiesDataDump.Converters;

namespace LostSkiesDataDump;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public partial class Plugin : BasePlugin
{
    private static T PropertyGetHelper<T>(T value, [CallerMemberName] string name = null) =>
        value
        ?? throw new InvalidOperationException(
            $"{MyPluginInfo.PLUGIN_GUID}'s {name} property has not been initialized"
        );

    internal static new ManualLogSource Log
    {
        get => PropertyGetHelper(field);
        private set => field = value;
    } = null;
    private static Harmony s_harmony = null;

    public static SerializationRoot SerializationRoot
    {
        [RequiresPreviewFeatures]
        get => field ??= new();
    } = null;

    [RequiresPreviewFeatures]
    public override void Load()
    {
        Log = base.Log;
        Log.LogInfo(
            $"Loading plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION}..."
        );
        ConfigBind();
        Log.LogInfo(
            $"  - config file: {Config.ConfigFilePath}{(File.Exists(Config.ConfigFilePath) ? "" : " (does not exist)")}"
        );
        Log.LogInfo($"  - base output directory: {BaseOutputDirectory}");
        Log.LogInfo($"  - icon output directory: {IconOutputDirectory}");
        Log.LogInfo($"  - text output file: {TextOutputFile}");
        s_harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        s_harmony.PatchAll(typeof(Patch));
        int index = 0;
        foreach (JsonConverter converter in SortedConverterSet.Default)
        {
            Log.LogDebug($"  Converter {index}: {converter}");
            index++;
        }
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

    [RequiresPreviewFeatures]
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
            foreach (JsonConverter converter in SortedConverterSet.Default)
                jsonSerializerOptions.Converters.Add(converter);
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
}
