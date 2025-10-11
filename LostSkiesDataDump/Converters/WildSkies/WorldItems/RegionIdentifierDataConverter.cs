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
using WildSkies.WorldItems;

namespace LostSkiesDataDump.Converters.WildSkies.WorldItems;

public class RegionIdentifierDataConverter<T> : BaseConverter<T> where T : RegionIdentifierData
{
    public override void WriteObjectBody(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        WriteProperty(writer, value.RegionID, options);
        WriteProperty(writer, value.RegionName, options);
        // public unsafe Region Region
        WriteProperty(writer, value.WelcomeLocalizedString, options);
        WriteProperty(writer, value.RegionNameLocalizedString, options);
    }
}
