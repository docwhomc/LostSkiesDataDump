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
public class GameObjectConverter
    : ObjectConverter<GameObject>,
        IConverterDefault<GameObjectConverter>
{
    public static new GameObjectConverter Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        GameObject value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value.transform, options);
        WriteProperty(writer, value.layer, options);
        WriteProperty(writer, value.active, options);
        WriteProperty(writer, value.activeSelf, options);
        WriteProperty(writer, value.activeInHierarchy, options);
        WriteProperty(writer, value.isStatic, options);
        WriteProperty(writer, value.isStaticBatchable, options);
        WriteProperty(writer, value.tag, options);
        WriteProperty(writer, value.scene, options);
        WriteProperty(writer, value.sceneCullingMask, options);
        WriteProperty(writer, value.gameObject, options);
        WriteArray(writer, value.GetComponents<Component>(), options);
        WriteProperty(writer, value.GetComponentCount(), options);
    }
}
