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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.Json;
using UnityEngine;

namespace LostSkiesDataDump.Converters.UnityEngine;

[RequiresPreviewFeatures]
public class LayerMaskConverter : BaseConverter<LayerMask>, IConverterDefault<LayerMaskConverter>
{
    public const int LayerNumberStart = 0;
    public const int LayerNumberCount = 32;
    public static LayerMaskConverter Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        LayerMask value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.m_Mask, options);
        WriteProperty(writer, value.value, options);
        WriteArray(writer, GetLayerNames(value.value), options, WriteLayers, "LayerToName()");
    }

    public static void WriteLayers(
        Utf8JsonWriter writer,
        Tuple<int, string> value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStartObject();
        WriteProperty(writer, value.Item1, options, "Layer");
        WriteProperty(writer, value.Item2, options, "Name");
        writer.WriteEndObject();
    }

    public static IEnumerable<Tuple<int, string>> GetLayerNames(int value) =>
        Enumerable
            .Range(LayerNumberStart, LayerNumberCount)
            .Where(layer => (value & (1 << layer)) != 0)
            .Select(GetLayerName);

    public static Tuple<int, string> GetLayerName(int layer) =>
        new(layer, LayerMask.LayerToName(layer));
}
