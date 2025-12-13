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
public class RectConverter : BaseConverter<Rect>, IConverterDefault<RectConverter>
{
    public static RectConverter Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        Rect value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.m_XMin, options);
        WriteProperty(writer, value.m_YMin, options);
        WriteProperty(writer, value.m_Width, options);
        WriteProperty(writer, value.m_Height, options);
        WriteProperty(writer, value.x, options);
        WriteProperty(writer, value.y, options);
        WriteProperty(writer, value.position, options);
        WriteProperty(writer, value.center, options);
        WriteProperty(writer, value.min, options);
        WriteProperty(writer, value.max, options);
        WriteProperty(writer, value.width, options);
        WriteProperty(writer, value.height, options);
        WriteProperty(writer, value.size, options);
        WriteProperty(writer, value.xMin, options);
        WriteProperty(writer, value.yMin, options);
        WriteProperty(writer, value.xMax, options);
        WriteProperty(writer, value.yMax, options);
        WriteProperty(writer, value.left, options);
        WriteProperty(writer, value.right, options);
        WriteProperty(writer, value.top, options);
        WriteProperty(writer, value.bottom, options);
        WriteProperty(writer, value.ToString(), options);
    }
}
