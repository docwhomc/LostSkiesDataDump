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

public class CCompendiumEntry : JsonConverter<CompendiumEntry>
{
    public override CompendiumEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, CompendiumEntry value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (!ConverterUtilities.WriteReference(writer, value, options))
        {
            writer.WriteString(ConverterUtilities.EncodeName(nameof(value.entryId), options), value.entryId);
            writer.WriteString(ConverterUtilities.EncodeName(nameof(value.mainCategoryId), options), value.mainCategoryId);
            writer.WriteString(ConverterUtilities.EncodeName(nameof(value.subCategoryId), options), value.subCategoryId);
            writer.WriteString(ConverterUtilities.EncodeName(nameof(value.mainCategoryRawString), options), value.mainCategoryRawString);
            writer.WriteString(ConverterUtilities.EncodeName(nameof(value.entryTitleRawString), options), value.entryTitleRawString);
            ConverterUtilities.WriteProperty(writer, value.mainCategoryString, nameof(value.mainCategoryString), options);
            ConverterUtilities.WriteProperty(writer, value.subCategoryString, nameof(value.subCategoryString), options);
            ConverterUtilities.WriteProperty(writer, value.entryTitleString, nameof(value.entryTitleString), options);
            ConverterUtilities.WriteProperty(writer, value.bodyTextString, nameof(value.bodyTextString), options);
            writer.WriteString(ConverterUtilities.EncodeName(nameof(value.itemId), options), value.itemId);
            writer.WriteString(ConverterUtilities.EncodeName(nameof(value.entityId), options), value.entityId);
            writer.WritePropertyName(ConverterUtilities.EncodeName(nameof(value.unlockIds), options));
            writer.WriteStartArray();
            foreach (var unlockId in value.unlockIds)
            {
                writer.WriteStringValue(unlockId);
            }
            writer.WriteEndArray();
            // TODO: Sprite icon
            writer.WriteString(ConverterUtilities.EncodeName(nameof(value.videoName), options), value.videoName);
            writer.WriteString(ConverterUtilities.EncodeName(nameof(value.conversationHistoryId), options), value.conversationHistoryId);
        }
        writer.WriteEndObject();
    }
}
