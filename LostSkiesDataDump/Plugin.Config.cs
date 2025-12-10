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
using UnityEngine;

namespace LostSkiesDataDump;

public partial class Plugin : BasePlugin
{
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

    private void ConfigBind()
    {
        ConfigBaseOutputDirectory = Config.Bind(
            "Output",
            "BaseDirectory",
            Path.Join(Application.dataPath, MyPluginInfo.PLUGIN_GUID),
            "The (base) directory that this plugin saves its output to."
        );
        ConfigIconOutputDirectory = Config.Bind(
            "Output",
            "IconDirectory",
            "icons",
            $"The directory that this plugin saves icons files to.  Its path is relative to that of the base output directory ({ConfigBaseOutputDirectory.Definition.Section}.{ConfigBaseOutputDirectory.Definition.Key})."
        );
        ConfigTextOutputFile = Config.Bind(
            "Output",
            "TextFile",
            "data.json",
            $"The file that this plugin saves text output to.  Its path is relative to that of the base output directory ({ConfigBaseOutputDirectory.Definition.Section}.{ConfigBaseOutputDirectory.Definition.Key})."
        );
    }
}
