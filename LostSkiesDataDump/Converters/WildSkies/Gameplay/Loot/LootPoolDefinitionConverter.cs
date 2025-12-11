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
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text.Json;
using WildSkies.Gameplay.Loot;
using WildSkies.IslandExport;
using Island = WildSkies.WorldItems.Island;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Loot;

[RequiresPreviewFeatures]
public class LootPoolDefinitionConverter<T>
    : BaseConverter<T>,
        IConverterDefault<LootPoolDefinitionConverter<LootPoolDefinition>>
    where T : LootPoolDefinition
{
    public static LootPoolDefinitionConverter<LootPoolDefinition> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.ID, options);
        WriteProperty(writer, value.LootPoolName, options);
        WriteProperty(writer, value.MinItemsToDrop, options);
        WriteProperty(writer, value.MaxItemsToDrop, options);
        WriteArray(writer, value.ItemList, options);
        WriteArray(writer, value.ItemSelectionWeightings, options);
        // public unsafe static Island.IslandDifficulty _defaultDifficulty
        // public unsafe static Island.IslandDifficulty DefaultDifficulty
        // public unsafe int GetNumItems()
        // public unsafe ItemDefinition GetRandomItem(out int amountToDrop, InventoryRarityType MinRarity, InventoryRarityType MaxRarity, IslandController islandController, int seed = -1, Il2CppSystem.Random random = null)
        // public unsafe bool HasValidItems(InventoryRarityType MinRarity, InventoryRarityType MaxRarity, IslandController islandController)
        WriteArray(writer, DropRate.GetDropRates(value), options, nameof(value.GetDropRate));
    }

    [Serializable]
    internal class DropRate(int itemIdx, Island.IslandDifficulty difficulty, string region, T value)
    {
#pragma warning disable IDE1006 // Naming Styles
        public int itemIdx { get; } = itemIdx;
        public Island.IslandDifficulty difficulty { get; } = difficulty;
        public string region { get; } = region;
        public float dropRate { get; } = value.GetDropRate(itemIdx, difficulty, region);
#pragma warning restore IDE1006 // Naming Styles

        internal static IEnumerable<DropRate> GetDropRates(T value)
        {
            List<string> regions = [null];
            foreach (Region region in Enum.GetValues(typeof(Region)))
            {
                regions.Add(Enum.GetName(region));
            }
            int numItems = value.ItemList.Count;
            for (int itemIdx = 0; itemIdx < numItems; itemIdx++)
            {
                foreach (
                    Island.IslandDifficulty difficulty in Enum.GetValues(
                        typeof(Island.IslandDifficulty)
                    )
                )
                {
                    foreach (string region in regions)
                    {
                        DropRate dropRate = null;
                        try
                        {
                            dropRate = new(itemIdx, difficulty, region, value);
                        }
                        catch (Exception e)
                        {
                            Plugin.Log.LogError(
                                $"new {nameof(DropRate)}({itemIdx}, {difficulty}, {region}, {value})"
                            );
                            Plugin.Log.LogDebug(e);
                        }
                        if (dropRate != null)
                            yield return dropRate;
                    }
                }
            }
        }
    }
}
