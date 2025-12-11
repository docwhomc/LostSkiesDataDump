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
using WildSkies.Gameplay.Crafting;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Crafting;

[RequiresPreviewFeatures]
public class CraftingComponentConverter<T>
    : BaseConverter<T>,
        IConverterDefault<CraftingComponentConverter<CraftingComponent>>
    where T : CraftingComponent
{
    public static CraftingComponentConverter<CraftingComponent> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.ComponentNameString, options);
        WriteProperty(writer, value.ResourceNameString, options);
        WriteProperty(writer, value.ItemId, options);
        WriteProperty(writer, value.QuantityNeeded, options);
        WriteProperty(writer, value.AcceptAnyItemOfType, options);
        WriteProperty(writer, value.WeightMultiplier, options);
        WriteArray(writer, value.MajorStat, options);
        WriteArray(writer, value.MinorStat, options);
    }
}
