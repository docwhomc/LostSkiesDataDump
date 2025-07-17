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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LostSkiesDataDump.Converters;

public abstract class BaseConverter<V>(bool reference) : JsonConverter<V>
{
    public const string ID_KEY = "$id";
    public const string REFERENCE_KEY = "$ref";
    public bool Reference = reference;

    public BaseConverter() : this(true) { }

    public override V Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, V value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }
        writer.WriteStartObject();
        try
        {
            if (!Reference || !WriteReference(writer, value, options))
                WriteObjectBody(writer, value, options);
        }
        catch (Exception e)
        {
            Plugin.Log.LogError($"Error writing {value}");
            Plugin.Log.LogError(e);
        }
        finally
        {
            writer.WriteEndObject();
        }
    }

    public abstract void WriteObjectBody(Utf8JsonWriter writer, V value, JsonSerializerOptions options);

    public static JsonEncodedText EncodeName(string name, JsonSerializerOptions options)
    {
        return JsonEncodedText.Encode(options.PropertyNamingPolicy?.ConvertName(name) ?? name);
    }

    public static JsonConverter<T> GetConverter<T>(JsonSerializerOptions options)
    {
        Type typeToConvert = typeof(T);
        if (typeToConvert == typeof(object))
        {
            Plugin.Log.LogWarning($"No converter for {typeToConvert} type");
            return null;
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
            return null;
        }
        if (converter is null)
        {
            Plugin.Log.LogDebug($"No JsonConverter for {typeToConvert} type in {options}");
            return null;
        }
        if (converter is JsonConverter<T> valueConverter)
            return valueConverter;
        Plugin.Log.LogWarning($"{converter} is not an instance of JsonConverter<{typeToConvert}>");
        return null;
    }

    public static void WriteArray<T>(Utf8JsonWriter writer, string name, IEnumerable<T> value, JsonSerializerOptions options)
    {
        JsonConverter<T> valueConverter = GetConverter<T>(options);
        try
        {
            writer.WriteStartArray(EncodeName(name, options));
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

    public static void WriteProperty<T>(Utf8JsonWriter writer, string name, T value, JsonSerializerOptions options)
    {
        JsonConverter<T> valueConverter = GetConverter<T>(options);
        try
        {
            writer.WritePropertyName(EncodeName(name, options));
            if (valueConverter is not null)
            {
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
            Plugin.Log.LogError($"Error writing property {name}: {value}");
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
