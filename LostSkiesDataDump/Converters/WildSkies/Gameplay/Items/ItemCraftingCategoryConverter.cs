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
using WildSkies.Gameplay.Items;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Items;

[RequiresPreviewFeatures]
public class ItemCraftingCategoryConverter<T>
    : ScriptableObjectConverter<T>,
        IConverterDefault<ItemCraftingCategoryConverter<ItemCraftingCategory>>
    where T : ItemCraftingCategory
{
    public static new ItemCraftingCategoryConverter<ItemCraftingCategory> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value._id, options);
        WriteProperty(writer, value._name, options);
        WriteProperty(writer, value._icon, options);
        WriteProperty(writer, value._subCategories, options);
        WriteProperty(writer, value.Id, options);
        WriteProperty(writer, value.Name, options);
        WriteProperty(writer, value.Icon, options);
        WriteProperty(writer, value.SubCategories, options);
    }
}
