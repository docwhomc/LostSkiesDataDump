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
using System.Runtime.Versioning;
using System.Text.Json;
using WildSkies.Gameplay.Items;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Items;

[RequiresPreviewFeatures]
public class ItemStatDefinitionConverter<T>
    : BaseConverter<T>,
        IConverterDefault<ItemStatDefinitionConverter<ItemStatDefinition>>
    where T : ItemStatDefinition
{
    public static ItemStatDefinitionConverter<ItemStatDefinition> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        try
        {
            WriteProperty(writer, value.ID, options);
            WriteProperty(writer, value.StatisticName, options);
            WriteProperty(writer, value.VariableName, options);
            WriteProperty(writer, value.CleanName, options);
            WriteProperty(writer, value.MinValue, options);
            WriteProperty(writer, value.MaxValue, options);
            WriteProperty(writer, value.MaterialAttributes, options);
            WriteProperty(writer, value.AttributeModifier, options);
        }
        catch (Exception e)
        {
            var message = $"Error serializing ItemStatDefinition: {value}";
            writer.WriteCommentValue(message);
            Plugin.Log.LogError(message);
            Plugin.Log.LogError(e);
        }
    }
}
