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
using System.Linq;
using System.Reflection;

namespace LostSkiesDataDump;

[Serializable]
public class PluginVersionInfo
{
    const string GIT_HASH_ATTRIBUTE_KEY = "GitHash";
    internal Assembly Assembly = Assembly.GetExecutingAssembly();
#pragma warning disable CA1822 // Mark members as static
    public string Guid => MyPluginInfo.PLUGIN_GUID;
#pragma warning restore CA1822 // Mark members as static
    public string GitHash =>
        Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(attr => attr.Key == GIT_HASH_ATTRIBUTE_KEY)
            ?.Value;
    public string InformationalVersion =>
        Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
#pragma warning disable CA1822 // Mark members as static
    public string Name => MyPluginInfo.PLUGIN_NAME;
    public string Version => MyPluginInfo.PLUGIN_VERSION;
#pragma warning restore CA1822 // Mark members as static
}
