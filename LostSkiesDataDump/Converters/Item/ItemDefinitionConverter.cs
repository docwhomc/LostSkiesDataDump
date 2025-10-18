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
using WildSkies.Gameplay.Items;

namespace LostSkiesDataDump.Converters.Item;

public class ItemDefinitionConverter<T> : BaseConverter<T>
    where T : ItemDefinition
{
    public ItemDefinitionConverter()
        : base(false) { }

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.Lock, options);
        WriteProperty(writer, value.ItemInGameAvailability, options);
        WriteProperty(writer, value.IsStashItem, options);
        WriteProperty(writer, value.ItemName, options);
        WriteProperty(writer, value.ItemNameRaw, options);
        WriteProperty(writer, value._airtableRecordRecordReference, options);
        WriteProperty(writer, value.ItemExclusionRadius, options);
        WriteProperty(writer, value.ItemCraftingCategoryName, options);
        WriteProperty(writer, value.ItemCraftingSubCategoryName, options);
        WriteProperty(writer, value.ItemCraftingCategoryId, options);
        WriteProperty(writer, value.ItemCraftingSubCategoryId, options);
        WriteProperty(writer, value.ItemRarity, options);
        WriteProperty(writer, value.LootAlignsToSurface, options);
        WriteArray(writer, value.ItemObtainedFrom, options);
        WriteProperty(writer, value.ItemRandomStatsDefinition, options);
        WriteProperty(writer, value.Upgradeable, options);
        WriteProperty(writer, value.AirtableItemId, options);
        WriteProperty(writer, value.AirtableItemUniqueId, options);
        WriteProperty(writer, value.ItemDescription, options);
        WriteProperty(writer, value.ItemTypeFlags, options);
        WriteProperty(writer, value.itemComponent, options);
        WriteArray(writer, value.ItemCompositionList.ToSystemEnumerable(), options);
        WriteProperty(writer, value.Rarity, options);
        WriteProperty(writer, value.AirtableRecordReference, options);
        // TODO: `public unsafe Sprite IconSprite`
        WriteProperty(writer, value.worldComponent, options);
        WriteProperty(writer, value.weaponComponent, options);
        WriteProperty(writer, value.throwableComponent, options);
        WriteProperty(writer, value.AmmoComponent, options);
        WriteProperty(writer, value.KnowledgeComponent, options);
        WriteProperty(writer, value.CustomisationComponent, options);
        WriteProperty(writer, value.AccessoryComponent, options);
        WriteProperty(writer, value.Id, options);
        WriteProperty(writer, value.UniqueId, options);
        WriteProperty(writer, value.Name, options);
        WriteProperty(writer, value.NameRaw, options);
        WriteProperty(writer, value.CraftingCategoryName, options);
        WriteProperty(writer, value.CraftingSubCategoryName, options);
        WriteProperty(writer, value.CraftingCategoryId, options);
        WriteProperty(writer, value.CraftingSubCategoryId, options);
        // TODO: `public unsafe virtual Sprite Icon`
        WriteProperty(writer, value.UseDynamicIconOnMainUI, options);
        WriteProperty(writer, value.Description, options);
        WriteProperty(writer, value.ExclusionRadius, options);
        WriteProperty(writer, value.InventoryRarityType, options);
        WriteArray(writer, value.ObtainedFrom, options);
        WriteProperty(writer, value.RandomStatsDefinition, options);
        WriteProperty(writer, value.InteractionName, options);
        WriteProperty(writer, value.InteractionId, options);
        // TODO: `public unsafe virtual Sprite InteractionSprite`
        WriteProperty(writer, value.InteractionRarity, options);
        // TODO: `public unsafe IEnumerator<ItemDefinition> GetEnumerator()`
        // TODO: `public unsafe List<BaseItemComponent> GetComponentsByType([DefaultParameterValue(null)] ItemTypes type)`
        // TODO: `public unsafe T GetComponent<T>() where T : BaseItemComponent`
    }
}
