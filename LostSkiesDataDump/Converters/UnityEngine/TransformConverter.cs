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
using UnityEngine;

namespace LostSkiesDataDump.Converters.UnityEngine;

[RequiresPreviewFeatures]
public class TransformConverter<T>
    : ComponentConverter<T>,
        IConverterDefault<TransformConverter<Transform>>
    where T : Transform
{
    public static new TransformConverter<Transform> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value.position, options);
        WriteProperty(writer, value.localPosition, options);
        WriteProperty(writer, value.eulerAngles, options);
        WriteProperty(writer, value.localEulerAngles, options);
        WriteProperty(writer, value.right, options);
        WriteProperty(writer, value.up, options);
        WriteProperty(writer, value.forward, options);
        WriteProperty(writer, value.rotation, options);
        WriteProperty(writer, value.localRotation, options);
        WriteProperty(writer, value.rotationOrder, options);
        WriteProperty(writer, value.localScale, options);
        WriteProperty(writer, value.parent, options);
        WriteProperty(writer, value.parentInternal, options);
        WriteProperty(writer, value.worldToLocalMatrix, options);
        WriteProperty(writer, value.localToWorldMatrix, options);
        WriteProperty(writer, value.root, options);
        WriteProperty(writer, value.childCount, options);
        WriteProperty(writer, value.lossyScale, options);
        WriteProperty(writer, value.hasChanged, options);
        WriteProperty(writer, value.hierarchyCapacity, options);
        WriteProperty(writer, value.hierarchyCount, options);
        WriteProperty(writer, value.constrainProportionsScale, options);
        WriteProperty(writer, value.GetSiblingIndex(), options);
        // public unsafe Transform GetChild(int index)
        WriteProperty(writer, value.GetChildCount(), options);
    }
}
