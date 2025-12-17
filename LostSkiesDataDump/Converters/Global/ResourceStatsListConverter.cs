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

using System.Runtime.Versioning;
using System.Text.Json;

namespace LostSkiesDataDump.Converters.Global;

[RequiresPreviewFeatures]
public class ResourceStatsListConverter<T>
    : BaseConverter<T>,
        IConverterDefault<ResourceStatsListConverter<ResourceStatsList>>
    where T : ResourceStatsList
{
    public static ResourceStatsListConverter<ResourceStatsList> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.Durability, options);
        WriteProperty(writer, value.Hardness, options);
        WriteProperty(writer, value.StressResistance, options);
        WriteProperty(writer, value.HeatResistance, options);
        WriteProperty(writer, value.ElectricalConductivity, options);
        // public unsafe float GetAttribute(string attributeName)
    }
}
