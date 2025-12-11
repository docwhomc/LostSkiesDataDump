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
public class CompendiumEntryConverter<T>
    : BaseConverter<T>,
        IConverterDefault<CompendiumEntryConverter<CompendiumEntry>>
    where T : CompendiumEntry
{
    public static CompendiumEntryConverter<CompendiumEntry> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.entryId, options);
        WriteProperty(writer, value.mainCategoryId, options);
        WriteProperty(writer, value.subCategoryId, options);
        WriteProperty(writer, value.mainCategoryRawString, options);
        WriteProperty(writer, value.entryTitleRawString, options);
        WriteProperty(writer, value.mainCategoryString, options);
        WriteProperty(writer, value.subCategoryString, options);
        WriteProperty(writer, value.entryTitleString, options);
        WriteProperty(writer, value.bodyTextString, options);
        WriteProperty(writer, value.itemId, options);
        WriteProperty(writer, value.entityId, options);
        WriteArray(writer, value.unlockIds, options);
        // TODO: Sprite icon
        WriteProperty(writer, value.videoName, options);
        WriteProperty(writer, value.conversationHistoryId, options);
    }
}
