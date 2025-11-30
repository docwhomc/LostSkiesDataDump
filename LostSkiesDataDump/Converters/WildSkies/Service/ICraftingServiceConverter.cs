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
using Il2CppSystem.Collections.Generic;
using LostSkiesDataDump.Converters.Interfaces;
using WildSkies.Gameplay.Crafting;
using WildSkies.Service;

namespace LostSkiesDataDump.Converters.WildSkies.Service;

[RequiresPreviewFeatures]
public class ICraftingServiceConverter<T>
    : BaseConverter<T>,
        IConverterDefault<ICraftingServiceConverter<T>>
    where T : ICraftingService
{
    public static JsonConverter Default { get; } =
        new ICraftingServiceConverter<ICraftingService>();

    public ICraftingServiceConverter()
        : base(false) { }

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.UseTestAssets, options);
        WriteArray(writer, value.RawSchematicDataList, options);
        WriteProperty(writer, value.FreeCrafting, options);
        // public unsafe virtual void LearnSchematic(string schematicId, int variationSeed)
        // public unsafe virtual void LearnCraftingMethod(CraftingMethod craftingMethod)
        // public unsafe virtual bool TryGetItemCategoryById(string id, out ItemCraftingCategory category)
        // public unsafe virtual bool TryGetItemSubcategory(string categoryId, string subcategoryId, out ItemCraftingSubCategory subCategory)
        // public unsafe virtual bool TryGetSchematicByOutputItemId(string itemId, out CraftableItemBlueprint schematic)
        // public unsafe virtual string GetSchematicItemIdFromUniqueId(ushort uniqueId)
        // public unsafe virtual ushort GetUniqueIdFromSchematicItemId(string itemId)
        // public unsafe virtual bool GetAllSchematicsOfType(CraftingMethod type, out List<CraftableItemBlueprint> schematics)
        value.GetAllSchematics(out List<CraftableItemBlueprint> schematics);
        WriteArray(writer, schematics, options, "GetAllSchematics()");
        // public unsafe virtual bool GetAssociatedSchematicForItem(string itemId, out CraftableItemBlueprint schematic)
    }
}
