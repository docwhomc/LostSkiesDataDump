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

namespace LostSkiesDataDump.Converters.Global;

[RequiresPreviewFeatures]
public class ShipPartMetalDescriptorConverter<T>
    : ScriptableObjectConverter<T>,
        IConverterDefault<ShipPartMetalDescriptorConverter<ShipPartMetalDescriptor>>
    where T : ShipPartMetalDescriptor
{
    public static new ShipPartMetalDescriptorConverter<ShipPartMetalDescriptor> Default { get; } =
        new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value.Name, options);
        WriteProperty(writer, value.Override, options);
        // WriteProperty(writer, value.BaseColorA, options);
        // WriteProperty(writer, value.BaseColorB, options);
        // WriteProperty(writer, value.PaintColourA, options);
        // WriteProperty(writer, value.PaintColourB, options);
        WriteProperty(writer, value.R_GlossMinMax, options);
        WriteProperty(writer, value.R_GlossMax, options);
        WriteProperty(writer, value.R_Metallic, options);
        WriteProperty(writer, value.CustomShader, options);
        WriteProperty(writer, value.CustomShaderName, options);
        // public unsafe static Il2CppStringArray ShaderNames
        // public unsafe static string OverridePropertyName
        // public unsafe static string BaseColorPropertyNameA
        // public unsafe static string BaseColorPropertyNameB
        // public unsafe static string GlossMinMaxPropertyName
        // public unsafe static string GlossMaxPropertyName
        // public unsafe static string PaintColourNameA
        // public unsafe static string PaintColourNameB
    }
}
