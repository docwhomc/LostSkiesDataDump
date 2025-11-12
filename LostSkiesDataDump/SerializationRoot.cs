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
using System.Text.Json.Serialization;
using LostSkiesDataDump.Converters.WildSkies.Service;
using WildSkies.Service;

namespace LostSkiesDataDump;

[Serializable]
public class SerializationRoot
{
    public SerializationRoot()
    {
        GameVersionInfo = new();
        PluginVersionInfo = new();
    }

    public GameVersionInfo GameVersionInfo { get; }
    public PluginVersionInfo PluginVersionInfo { get; }

    [JsonConverter(typeof(ICompendiumServiceConverter<ICompendiumService>))]
    public ICompendiumService CompendiumService { get; set; }

    [JsonConverter(typeof(IItemServiceConverter<IItemService>))]
    public IItemService ItemService { get; set; }

    [JsonConverter(typeof(WorldRegionServiceConverter<WorldRegionService>))]
    public WorldRegionService WorldRegionService { get; set; }
}
