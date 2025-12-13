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
public class Matrix4x4Converter : BaseConverter<Matrix4x4>, IConverterDefault<Matrix4x4Converter>
{
    public static Matrix4x4Converter Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        Matrix4x4 value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.m00, options);
        WriteProperty(writer, value.m01, options);
        WriteProperty(writer, value.m02, options);
        WriteProperty(writer, value.m03, options);
        WriteProperty(writer, value.m10, options);
        WriteProperty(writer, value.m11, options);
        WriteProperty(writer, value.m12, options);
        WriteProperty(writer, value.m13, options);
        WriteProperty(writer, value.m20, options);
        WriteProperty(writer, value.m21, options);
        WriteProperty(writer, value.m22, options);
        WriteProperty(writer, value.m23, options);
        WriteProperty(writer, value.m30, options);
        WriteProperty(writer, value.m31, options);
        WriteProperty(writer, value.m32, options);
        WriteProperty(writer, value.m33, options);
        WriteProperty(writer, value.ToString(), options);
    }
}
