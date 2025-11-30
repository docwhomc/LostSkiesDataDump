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
using LostSkiesDataDump.Converters.Interfaces;
using WildSkies.Gameplay.Container;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Container;

[RequiresPreviewFeatures]
public class ContainerDefinitionConverter<T>
    : BaseConverter<T>,
        IConverterDefault<ContainerDefinitionConverter<T>>
    where T : ContainerDefinition
{
    public static JsonConverter Default { get; } =
        new ContainerDefinitionConverter<ContainerDefinition>();

    public class PoolSlotConverter<TP> : BaseConverter<TP>
        where TP : ContainerDefinition.PoolSlot
    {
        public override void WriteObjectBody(
            Utf8JsonWriter writer,
            TP value,
            JsonSerializerOptions options
        )
        {
            WriteProperty(writer, value._poolIndex, options);
            WriteProperty(writer, value._curSelections, options);
            WriteProperty(writer, value.PoolIndex, options);
            WriteProperty(writer, value.CurSelections, options);
        }
    }

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.ID, options);
        WriteProperty(writer, value.ContainerName, options);
        WriteProperty(writer, value.Category, options);
        WriteProperty(writer, value.Type, options);
        WriteProperty(writer, value.Description, options);
        WriteProperty(writer, value.MinSelections, options);
        WriteProperty(writer, value.MaxSelections, options);
        WriteProperty(writer, value.MinRarity, options);
        WriteProperty(writer, value.MaxRarity, options);
        WriteArray(writer, value.LootPoolList, options);
        WriteArray(writer, value.LootPoolSelectionWeightings, options);
        WriteArray(writer, value.LootPoolMaxSelections, options);
        // public unsafe static Island.IslandDifficulty _defaultDifficulty
        // public unsafe List<LootPoolDefinition> GetRandomLootPools(IslandController islandController)
        // public unsafe List<LootPoolDefinition> GetRandomLootPools(IslandController islandController, out List<int> selectedWeightings, Il2CppSystem.Random random = null)
        // public unsafe static LootPoolDefinition SelectWeightedLootPool(List<LootPoolDefinition> selectedList, List<int> selectedWeightings, Il2CppSystem.Random random = null)
        // public unsafe int GetSelectionWeighting(int itemIdx, Island.IslandDifficulty difficulty)
    }
}
