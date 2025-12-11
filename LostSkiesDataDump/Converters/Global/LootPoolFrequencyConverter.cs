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
public class LootPoolFrequencyConverter
    : BaseConverter<LootPoolFrequency>,
        IConverterDefault<LootPoolFrequencyConverter>
{
    public static LootPoolFrequencyConverter Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        LootPoolFrequency value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.LootPoolDefinition, options);
        WriteProperty(writer, value.Frequency, options);
        WriteProperty(writer, value.MaxPerSpawner, options);
    }
}
