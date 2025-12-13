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
using WildSkies.Gameplay.Crafting;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Crafting;

[RequiresPreviewFeatures]
public class CraftableItemBlueprintConverter<T>
    : ScriptableObjectConverter<T>,
        IConverterDefault<CraftableItemBlueprintConverter<CraftableItemBlueprint>>
    where T : CraftableItemBlueprint
{
    public static new CraftableItemBlueprintConverter<CraftableItemBlueprint> Default { get; } =
        new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value.Id, options);
        WriteProperty(writer, value.UniqueId, options);
        WriteProperty(writer, value.CraftingCategoryId, options);
        WriteProperty(writer, value.CraftingSubCategoryId, options);
        WriteProperty(writer, value.CraftingCategoryLocName, options);
        WriteProperty(writer, value.CraftingSubCategoryLocName, options);
        WriteProperty(writer, value.CraftingMethod, options);
        WriteProperty(writer, value.SchematicType, options);
        WriteProperty(writer, value.Components, options);
        WriteProperty(writer, value.OutputItemId, options);
        WriteProperty(writer, value.OutputQuantity, options);
        WriteProperty(writer, value.TimeToCraft, options);
        WriteProperty(writer, value.ItemLevels, options);
        WriteProperty(writer, value.SalvageAmount, options);
        WriteProperty(writer, value.LearnCost, options);
        WriteProperty(writer, value.LearnMethod, options);
        WriteProperty(writer, value.TutorialToUnlockOnLearned, options);
    }
}
