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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LostSkiesDataDump.Converters.Compendium;

public class CCompendiumCategory : JsonConverter<CompendiumCategory>
{
    public override CompendiumCategory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, CompendiumCategory value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (!ConverterUtilities.WriteReference(writer, value, options))
        {
            writer.WriteString(ConverterUtilities.EncodeName(nameof(value.Id), options), value.Id);
            ConverterUtilities.WriteProperty(writer, value.Name, nameof(value.Name), options);
            writer.WriteBoolean(ConverterUtilities.EncodeName(nameof(value.IsMainCategory), options), value.IsMainCategory);
            writer.WriteNumber(ConverterUtilities.EncodeName(nameof(value.PreferredIndex), options), value.PreferredIndex);
            writer.WritePropertyName(ConverterUtilities.EncodeName(nameof(value.SubCategories), options));
            writer.WriteStartArray();
            foreach (var subCategory in value.SubCategories)
                writer.WriteStringValue(subCategory.Id);
            writer.WriteEndArray();
        }
        writer.WriteEndObject();
    }
}
