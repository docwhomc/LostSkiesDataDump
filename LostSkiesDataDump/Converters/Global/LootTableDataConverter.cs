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
using LostSkiesDataDump.Converters.UnityEngine;

namespace LostSkiesDataDump.Converters.Global;

[RequiresPreviewFeatures]
public class LootTableDataConverter<T>
    : ScriptableObjectConverter<T>,
        IConverterDefault<LootTableDataConverter<LootTableData>>
    where T : LootTableData
{
    public static new LootTableDataConverter<LootTableData> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value.SubBiomeType, options);
        WriteProperty(writer, value.NumPerSquareKilometreRange, options);
        WriteProperty(writer, value.MaxValue, options);
        WriteProperty(writer, value.ContainerDefinitions, options);
        WriteProperty(writer, value.LootPoolFrequencyList, options);
        // public unsafe ContainerDefinition GetRandomContainer(Il2CppSystem.Random random)
        // public unsafe LootPoolDefinition GetRandomLootPool(Il2CppSystem.Random random)
    }
}
