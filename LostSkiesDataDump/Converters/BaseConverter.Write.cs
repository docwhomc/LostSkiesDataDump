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
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LostSkiesDataDump.Converters;

public abstract partial class BaseConverter<T> : JsonConverter<T>
{
    public static JsonEncodedText EncodeName(string name, JsonSerializerOptions options)
    {
        return JsonEncodedText.Encode(options.PropertyNamingPolicy?.ConvertName(name) ?? name);
    }

    public static Action<Utf8JsonWriter, V, JsonSerializerOptions> GetSerializer<V>(JsonSerializerOptions options)
    {
        Type typeToConvert = typeof(V);
        if (typeToConvert == typeof(object))
        {
            Plugin.Log.LogWarning($"No converter for {typeToConvert} type");
            return JsonSerializer.Serialize;
        }
        JsonConverter converter;
        try
        {
            converter = options?.GetConverter(typeToConvert);
        }
        catch (Exception e)
        {
            Plugin.Log.LogError($"Error getting converter for {typeToConvert} type");
            Plugin.Log.LogError(e);
            return JsonSerializer.Serialize;
        }
        if (converter is null)
        {
            Plugin.Log.LogDebug($"No JsonConverter for {typeToConvert} type in {options}");
            return JsonSerializer.Serialize;
        }
        if (converter is JsonConverter<V> valueConverter)
            return valueConverter.Write;
        Plugin.Log.LogWarning($"{converter} is not an instance of JsonConverter<{typeToConvert}>");
        return JsonSerializer.Serialize;
    }

    [ObsoleteAttribute($"{nameof(WriteArray)}<V>(Utf8JsonWriter, string, IEnumerable<V>, JsonSerializerOptions) is obsolete, use {nameof(WriteArray)}<V>(Utf8JsonWriter, IEnumerable<V>, JsonSerializerOptions, [string name = null]) instead.")]
    public static void WriteArray<V>(Utf8JsonWriter writer, string name, IEnumerable<V> value, JsonSerializerOptions options)
    {
        WriteArray(writer, value, options, name);
    }

    public static void WriteArray<V>(Utf8JsonWriter writer, IEnumerable<V> value, JsonSerializerOptions options, [CallerArgumentExpression(nameof(value))] string name = null)
    {
        var serializer = GetSerializer<V>(options);
        try
        {
            writer.WriteStartArray(EncodeName(name, options));
            foreach (var element in value)
                serializer(writer, element, options);
            writer.WriteEndArray();
        }
        catch (Exception e)
        {
            var message = $"Error writing array {name} with value {value}";
            writer.WriteCommentValue(message);
            Plugin.Log.LogError(message);
            Plugin.Log.LogError(e);
        }
    }

    [ObsoleteAttribute($"{nameof(WriteProperty)}<V>(Utf8JsonWriter, string, IEnumerable<V>, JsonSerializerOptions) is obsolete, use {nameof(WriteArray)}<V>(Utf8JsonWriter, IEnumerable<V>, JsonSerializerOptions, [string name = null]) instead.")]
    public static void WriteProperty<V>(Utf8JsonWriter writer, string name, V value, JsonSerializerOptions options)
    {
        WriteProperty(writer, value, options, name);
    }

    public static void WriteProperty<V>(Utf8JsonWriter writer, V value, JsonSerializerOptions options, [CallerArgumentExpression(nameof(value))] string name = null)
    {
        var serializer = GetSerializer<V>(options);
        try
        {
            writer.WritePropertyName(EncodeName(name, options));
            serializer(writer, value, options);
        }
        catch (Exception e)
        {
            var message = $"Error writing property {name} with value {value}";
            writer.WriteCommentValue(message);
            Plugin.Log.LogError(message);
            Plugin.Log.LogError(e);
        }
    }

    public static void WriteValue<V>(Utf8JsonWriter writer, V value, JsonSerializerOptions options)
    {
        var serializer = GetSerializer<V>(options);
        try
        {
            serializer(writer, value, options);
        }
        catch (Exception e)
        {
            var message = $"Error writing value {value}";
            writer.WriteCommentValue(message);
            Plugin.Log.LogError(message);
            Plugin.Log.LogError(e);
        }
    }
}
