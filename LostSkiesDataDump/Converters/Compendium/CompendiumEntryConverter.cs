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

namespace LostSkiesDataDump.Converters.Compendium;

public class CompendiumEntryConverter<T> : BaseConverter<T> where T : CompendiumEntry
{
    public override void WriteObjectBody(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteString(EncodeName(nameof(value.entryId), options), value.entryId);
        writer.WriteString(EncodeName(nameof(value.mainCategoryId), options), value.mainCategoryId);
        writer.WriteString(EncodeName(nameof(value.subCategoryId), options), value.subCategoryId);
        writer.WriteString(EncodeName(nameof(value.mainCategoryRawString), options), value.mainCategoryRawString);
        writer.WriteString(EncodeName(nameof(value.entryTitleRawString), options), value.entryTitleRawString);
        WriteProperty(writer, nameof(value.mainCategoryString), value.mainCategoryString, options);
        WriteProperty(writer, nameof(value.subCategoryString), value.subCategoryString, options);
        WriteProperty(writer, nameof(value.entryTitleString), value.entryTitleString, options);
        WriteProperty(writer, nameof(value.bodyTextString), value.bodyTextString, options);
        writer.WriteString(EncodeName(nameof(value.itemId), options), value.itemId);
        writer.WriteString(EncodeName(nameof(value.entityId), options), value.entityId);
        WriteArray(writer, nameof(value.unlockIds), value.unlockIds, options);
        // TODO: Sprite icon
        writer.WriteString(EncodeName(nameof(value.videoName), options), value.videoName);
        writer.WriteString(EncodeName(nameof(value.conversationHistoryId), options), value.conversationHistoryId);
    }
}
