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

using System.Text.Json;
using global::WildSkies.Service;

namespace LostSkiesDataDump.Converters.WildSkies.Service;

public class WorldRegionServiceConverter<T> : BaseConverter<T>
    where T : WorldRegionService
{
    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        if (!value._initialised)
            Plugin.Log.LogWarning($"attempting to serialize {value}, which hasn't initialized");
        // public unsafe static float RegionUpdateInterval
        // public unsafe Dictionary<int, RegionIdentifierData> _regionIdentifiers
        WriteProperty(writer, value._regionIdentifiers, options);
        // public unsafe List<RegionalEntity> RegionalEntities
        // public unsafe Map _worldMap
        // public unsafe WorldLoadingService _worldLoadingService
        // public unsafe HostControl _hostControl
        // public unsafe SkyMapService _skyMapService
        // public unsafe RegionIdentifierData GetRegionIdentifierDataById(int id)
    }
}
