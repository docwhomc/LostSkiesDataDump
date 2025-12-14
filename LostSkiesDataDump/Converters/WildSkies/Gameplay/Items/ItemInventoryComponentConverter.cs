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
using wgi = WildSkies.Gameplay.Items;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Items;

[RequiresPreviewFeatures]
public class ItemInventoryComponentConverter<T>
    : BaseItemComponentConverter<T>,
        IConverterDefault<ItemInventoryComponentConverter<wgi.ItemInventoryComponent>>
    where T : wgi.ItemInventoryComponent
{
    public static new ItemInventoryComponentConverter<wgi.ItemInventoryComponent> Default { get; } =
        new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        // public new unsafe static ItemTypes ClassItemType
        WriteProperty(writer, value.ShapeSprite, options);
        // // public unsafe Il2CppObjectBase _shapeArray
        // WriteProperty(writer, value._shapeArray, options);
        WriteProperty(writer, value._shapeRelativePositions, options);
        WriteProperty(writer, value.MaxStackSize, options);
        WriteProperty(writer, value.ShapeType, options);
        WriteProperty(writer, value.AttachmentPositionsList, options);
        WriteProperty(writer, value.ItemType, options);
        WriteProperty(writer, value.IsStackable, options);
        WriteProperty(writer, value.ShapeRelativePositions, options);
        // // public unsafe Il2CppObjectBase ShapeArray
        // WriteProperty(writer, value.ShapeArray, options);
        WriteProperty(writer, value.GetRotatedShape(), options);
        WriteProperty(writer, value.GetRotateDirection(), options);
        WriteProperty(writer, value.CanBeRotated(), options);
    }
}
