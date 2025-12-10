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
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using LostSkiesDataDump.Converters;
using LostSkiesDataDump.Converters.UnityEngine.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace LostSkiesDataDump;

public partial class Plugin : BasePlugin
{
    private const string OutputSection = "Output";

    protected static ConfigEntry<string> ConfigBaseOutputDirectory
    {
        get => PropertyGetHelper(field);
        private set => field = value;
    } = null;
    public static string BaseOutputDirectory => ConfigBaseOutputDirectory.Value;

    protected static ConfigEntry<string> ConfigIconOutputDirectory
    {
        get => PropertyGetHelper(field);
        private set => field = value;
    } = null;
    public static string IconOutputDirectory =>
        Path.Join(BaseOutputDirectory, ConfigIconOutputDirectory.Value);

    protected static ConfigEntry<string> ConfigTextOutputFile
    {
        get => PropertyGetHelper(field);
        private set => field = value;
    } = null;
    public static string TextOutputFile =>
        Path.Join(BaseOutputDirectory, ConfigTextOutputFile.Value);

    private const string VerbositySection = "Verbosity";

    protected static ConfigEntry<bool> ConfigLogBadLocalizedString
    {
        get => PropertyGetHelper(field);
        private set => field = value;
    } = null;
    public static bool LogBadLocalizedString => ConfigLogBadLocalizedString.Value;

    protected static ConfigEntry<bool> ConfigLogCleanerCacheEntry
    {
        get => PropertyGetHelper(field);
        private set => field = value;
    } = null;
    public static bool LogCleanerCacheEntry => ConfigLogCleanerCacheEntry.Value;

    protected static ConfigEntry<bool> ConfigLogFactoryCreateConverter
    {
        get => PropertyGetHelper(field);
        private set => field = value;
    } = null;
    public static bool LogFactoryCreateConverter => ConfigLogFactoryCreateConverter.Value;

    private void ConfigBind()
    {
        ConfigBaseOutputDirectory = Config.Bind(
            OutputSection,
            "BaseDirectory",
            Path.Join(Application.dataPath, MyPluginInfo.PLUGIN_GUID),
            "The (base) directory that this plugin saves its output to."
        );
        ConfigIconOutputDirectory = Config.Bind(
            OutputSection,
            "IconDirectory",
            "icons",
            $"The directory that this plugin saves icons files to.  Its path is relative to that of the base output directory ({ConfigBaseOutputDirectory.Definition})."
        );
        ConfigTextOutputFile = Config.Bind(
            OutputSection,
            "TextFile",
            "data.json",
            $"The file that this plugin saves text output to.  Its path is relative to that of the base output directory ({ConfigBaseOutputDirectory.Definition})."
        );
        ConfigLogBadLocalizedString = Config.Bind(
            VerbositySection,
            "LogBadLocalizedString",
            false,
            $"If true, log when a {nameof(LocalizedStringConverter<>)} attempts to serialize a {nameof(LocalizedString)} that does not specify a table reference or collection. Otherwise, do not log such messages."
        );
        ConfigLogCleanerCacheEntry = Config.Bind(
            VerbositySection,
            "LogCleanerCacheEntry",
            false,
            $"If true, log when a {nameof(BaseConverter<>)}.{nameof(BaseConverter<>.CleanName)}() adds an entry to the name cleaner cache. Otherwise, do not log such messages."
        );
        ConfigLogFactoryCreateConverter = Config.Bind(
            VerbositySection,
            "LogFactoryCreateConverter",
            false,
            $"If true, log when a {nameof(BaseConverterFactory)}.{nameof(BaseConverterFactory.CreateConverter)}() creates and caches a new converter. Otherwise, do not log such messages."
        );
    }
}
