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
using System.Text.Json.Serialization;
using LostSkiesDataDump.Converters.Interfaces;
using UnityEngine.Localization;
using WildSkies.Gameplay.Items;
using ics = Il2CppSystem;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Items;

[RequiresPreviewFeatures]
public class ItemProfileConverter<T> : BaseConverter<T>, IConverterDefault<ItemProfileConverter<T>>
    where T : ItemProfile
{
    public static JsonConverter Default { get; } = new ItemProfileConverter<ItemProfile>();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteArray(
            writer,
            value.GetItemParameters()?.ToSystemEnumerable(),
            options,
            WriteItemParameter
        );
    }

    public virtual void WriteItemParameter(
        Utf8JsonWriter writer,
        ics.ValueTuple<LocalizedString, string, float, float> value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStartArray();
        try
        {
            WriteValue(writer, value.Item1, options);
            writer.WriteStringValue(value.Item2);
            writer.WriteNumberValue(value.Item3);
            writer.WriteNumberValue(value.Item4);
        }
        catch (Exception e)
        {
            var message = $"Error serializing item parameter {value}";
            writer.WriteCommentValue(message);
            Plugin.Log.LogError(message);
            Plugin.Log.LogDebug(e);
        }
        finally
        {
            writer.WriteEndArray();
        }
    }
}
