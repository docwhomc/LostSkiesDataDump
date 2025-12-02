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
using System.Text.Json.Serialization;
using WildSkies.Service;

namespace LostSkiesDataDump.Converters.WildSkies.Service;

[RequiresPreviewFeatures]
public class LootTableServiceConverter<T>
    : BaseConverter<T>,
        IConverterDefault<LootTableServiceConverter<T>>
    where T : LootTableService
{
    public static JsonConverter Default { get; } =
        new LootTableServiceConverter<LootTableService>();

    public LootTableServiceConverter()
        : base(false) { }

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value._lootTables, options);
        WriteProperty(writer, value.UseTestAssets, options);
        // public unsafe static string AddressableLocationGameKey
        WriteProperty(writer, value.ServiceErrorCode, options);
        WriteProperty(writer, value.FinishedInitialisation, options);
        WriteProperty(writer, value.CanGameRunIfServiceFailed, options);
        WriteProperty(writer, value.AddressableLocationKey, options);
        // public unsafe virtual bool TryGetLootTable(string lootTableName, out LootTable lootTable)
    }
}
