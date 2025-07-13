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
using System.Text.Json.Serialization;
using Il2CppSystem.Collections.Generic;

namespace LostSkiesDataDump.Converters;

public abstract class BaseConverter<V> : JsonConverter<V>
{
    public const string ID_KEY = "$id";
    public const string REFERENCE_KEY = "$ref";

    public override V Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, V value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (!WriteReference(writer, value, options))
            WriteObjectBody(writer, value, options);
        writer.WriteEndObject();
    }

    public abstract void WriteObjectBody(Utf8JsonWriter writer, V value, JsonSerializerOptions options);

    public static JsonEncodedText EncodeName(string name, JsonSerializerOptions options)
    {
        return JsonEncodedText.Encode(options.PropertyNamingPolicy?.ConvertName(name) ?? name);
    }

    public static JsonConverter<T> GetConverter<T>(string name, JsonSerializerOptions options)
    {
        Type typeToConvert = typeof(T);
        if (typeToConvert == typeof(object))
        {
            Plugin.Log.LogWarning($"GetConverter<{typeToConvert}>({name}, {options}) -> null: no converter for object type");
            return null;
        }
        JsonConverter converter;
        try
        {
            converter = options?.GetConverter(typeToConvert);
        }
        catch (Exception e)
        {
            Plugin.Log.LogError($"GetConverter<{typeToConvert}>({name}, {options}) -> null: unable to get converter");
            Plugin.Log.LogError(e);
            return null;
        }
        if (converter is null)
        {
            Plugin.Log.LogWarning($"GetConverter<{typeToConvert}>({name}, {options}) -> null: options do not contain a JsonConverter for {typeToConvert}");
            return null;
        }
        if (converter is JsonConverter<T> valueConverter)
            return valueConverter;
        Plugin.Log.LogWarning($"GetConverter<{typeToConvert}>({name}, {options}): {converter} is not an instance of JsonConverter<{typeToConvert}>");
        return null;
    }

    public static void WriteArray<T>(Utf8JsonWriter writer, List<T> value, string name, JsonSerializerOptions options)
    {
        JsonConverter<T> valueConverter = GetConverter<T>(name, options);
        var encodedName = EncodeName(name, options);
        try
        {
            writer.WritePropertyName(encodedName);
            writer.WriteStartArray();
            if (valueConverter is not null)
            {
                foreach (var element in value)
                    valueConverter.Write(writer, element, options);
            }
            else
            {
                Plugin.Log.LogDebug($"Falling back to JsonSerializer.Serialize() for elements of {value}");
                foreach (var element in value)
                    JsonSerializer.Serialize(writer, element, options);
            }
            writer.WriteEndArray();
        }
        catch (Exception e)
        {
            Plugin.Log.LogError(e);
        }
    }

    public static void WriteProperty<T>(Utf8JsonWriter writer, T value, string name, JsonSerializerOptions options)
    {
        JsonConverter<T> valueConverter = GetConverter<T>(name, options);
        var encodedName = EncodeName(name, options);
        try
        {
            if (valueConverter is not null)
            {
                writer.WritePropertyName(encodedName);
                valueConverter.Write(writer, value, options);
            }
            else
            {
                Plugin.Log.LogDebug($"Falling back to JsonSerializer.Serialize() for value {value}");
                JsonSerializer.Serialize(writer, value, options);
            }
        }
        catch (Exception e)
        {
            Plugin.Log.LogError(e);
        }
    }

    // Returns true if REFERENCE_KEY is written, false if ID ID_KEY is written or if there is no reference.
    public static bool WriteReference<T>(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
            return false;
        if (options.ReferenceHandler?.CreateResolver() is not ReferenceResolver resolver)
            return false;
        var reference = resolver.GetReference(value, out bool alreadyExists);
        if (reference is null)
            return false;
        writer.WriteString(alreadyExists ? REFERENCE_KEY : ID_KEY, reference);
        return alreadyExists;
    }
}
