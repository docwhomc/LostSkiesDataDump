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

using System;
using System.Text.Json;

namespace LostSkiesDataDump.Converters.System;

public class IntPtrConverter : BaseConverter<IntPtr>
{
    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        IntPtr value,
        JsonSerializerOptions options
    )
    {
        Plugin.Log.LogError($"attempting to serialize {nameof(IntPtr)} {value}");
        WriteProperty(writer, value.GetHashCode(), options);
        WriteProperty(writer, value.ToInt64(), options);
        WriteProperty(writer, value.ToString(), options);
    }
}
