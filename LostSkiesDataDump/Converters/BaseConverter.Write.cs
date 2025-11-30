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
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using icg = Il2CppSystem.Collections.Generic;

namespace LostSkiesDataDump.Converters;

[RequiresPreviewFeatures]
public abstract partial class BaseConverter<T> : JsonConverter<T>
{
    /// <summary>
    /// <c>s_nameCleaner</c> is a regular expression used by <see cref="CleanName"/>.
    /// </summary>
    private static readonly Regex s_nameCleaner = new(
        "^(?:value\\.)?(.*?)(?:\\??\\.(?:Select\\(.*\\)|ToSystemEnumerable\\(\\)))?$"
    );

    /// <summary>
    /// <c>s_nameCleanerCache</c> is a cache of cleaned names used by <see cref="CleanName"/>.
    /// </summary>
    private static readonly Dictionary<string, string> s_nameCleanerCache = [];

    /// <summary>
    /// Clears the cleaned name cache used by <see cref="CleanName"/>.
    /// </summary>
    public static void ClearNameCleanerCache()
    {
        s_nameCleanerCache.Clear();
    }

    /// <summary>
    /// Cleans a specified name.
    /// <para>
    /// If present, strips a prefix <c>value.</c>.  If present, strips a suffix beginning with <c>.Select(</c> or a suffix <c>.ToSystemEnumerable()</c>.
    /// </para>
    /// <para>
    /// Results are cached in a dictionary.  The cache dictionary can be cleared by calling <see cref="ClearNameCleanerCache"/>.
    /// </para>
    /// </summary>
    /// <param name="name">The name to be cleaned.</param>
    /// <returns>The cleaned name.</returns>
    public static string CleanName(string name)
    {
        if (s_nameCleanerCache.TryGetValue(name, out string cached))
            return cached;
        var match = s_nameCleaner.Match(name);
        var cacheKey = name;
        if (match.Success)
            name = match.Groups[1].Value;
        else
            Plugin.Log.LogWarning($"name `{name}` didn't match pattern");
        Plugin.Log.LogDebug($"Adding name cleaner cache entry: `{cacheKey}` -> `{name}`");
        s_nameCleanerCache.Add(cacheKey, name);
        return name;
    }

    /// <summary>
    /// Cleans a specified name, formats it according to <paramref name="options"/>, and encodes it as JSON string.
    /// </summary>
    /// <seealso cref="CleanName"/>
    /// <param name="name">The name to clean and convert to JSON encoded text.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The encoded JSON text.</returns>
    public static JsonEncodedText EncodeName(string name, JsonSerializerOptions options)
    {
        name = CleanName(name);
        return JsonEncodedText.Encode(options.PropertyNamingPolicy?.ConvertName(name) ?? name);
    }

    /// <summary>
    /// Gets a serializer for type <typeparamref name="V"/>.
    /// </summary>
    /// <typeparam name="V">The type to return a converter for.</typeparam>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The <see cref="Write"/> method of the first converter that supports the given type, or <see cref="JsonSerializer.Serialize"/> if there is no converter.</returns>
    public static Action<Utf8JsonWriter, V, JsonSerializerOptions> GetSerializer<V>(
        JsonSerializerOptions options
    )
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

    /// <summary>
    ///   Writes a property name specified as a string and an array value as part of a name/value pair of a JSON object.
    /// </summary>
    /// <typeparam name="V">The type of the elements of the array value to convert to JSON.</typeparam>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The array value to written as a JSON array as part of a name/value pair.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <param name="name">The UTF-16 encoded property name of the JSON object to be processed by <see cref="EncodeName"/> written as UTF-8.  If null (the default value), the expression for <paramref name="value"/> is used.</param>
    public static void WriteArray<V>(
        Utf8JsonWriter writer,
        IEnumerable<V> value,
        JsonSerializerOptions options,
        [CallerArgumentExpression(nameof(value))] string name = null
    )
    {
        var serializer = GetSerializer<V>(options);
        WriteArray(writer, value, options, serializer, name);
    }

    /// <summary>
    ///   Writes a property name specified as a string and an array value as part of a name/value pair of a JSON object.
    /// </summary>
    /// <typeparam name="V">The type of the elements of the array value to convert to JSON.</typeparam>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The array value to written as a JSON array as part of a name/value pair.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <param name="serializer">A serializer for type <typeparamref name="V"/>.</param>
    /// <param name="name">The UTF-16 encoded property name of the JSON object to be processed by <see cref="EncodeName"/> written as UTF-8.  If null (the default value), the expression for <paramref name="value"/> is used.</param>
    public static void WriteArray<V>(
        Utf8JsonWriter writer,
        IEnumerable<V> value,
        JsonSerializerOptions options,
        Action<Utf8JsonWriter, V, JsonSerializerOptions> serializer,
        [CallerArgumentExpression(nameof(value))] string name = null
    )
    {
        try
        {
            writer.WriteStartArray(EncodeName(name, options));
            if (value is not null)
            {
                int index = 0;
                foreach (var element in value)
                {
                    try
                    {
                        serializer(writer, element, options);
                    }
                    catch (Exception e)
                    {
                        var message =
                            $"Error serializing element {index} ({element}) of array {name} ({value})";
                        writer.WriteCommentValue(message);
                        Plugin.Log.LogError(message);
                        Plugin.Log.LogError(e);
                    }
                    index++;
                }
            }
            writer.WriteEndArray();
        }
        catch (Exception e)
        {
            var message = $"Error serializing array {name} ({value})";
            writer.WriteCommentValue(message);
            Plugin.Log.LogError(message);
            Plugin.Log.LogError(e);
        }
    }

    /// <summary>
    ///   Writes a property name specified as a string and an list value as part of a name/value pair of a JSON object.
    /// </summary>
    /// <typeparam name="V">The type of the elements of the array value to convert to JSON.</typeparam>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The list value to written as a JSON array as part of a name/value pair.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <param name="name">The UTF-16 encoded property name of the JSON object to be processed by <see cref="EncodeName"/> written as UTF-8.  If null (the default value), the expression for <paramref name="value"/> is used.</param>
    public static void WriteArray<V>(
        Utf8JsonWriter writer,
        icg.List<V> value,
        JsonSerializerOptions options,
        [CallerArgumentExpression(nameof(value))] string name = null
    )
    {
        WriteArray(writer, value.ToSystemEnumerable(), options, name);
    }

    /// <summary>
    ///   Writes a property name specified as a string and a value as part of a name/value pair of a JSON object.
    /// </summary>
    /// <typeparam name="V">The type of the value to convert to JSON.</typeparam>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to written as an appropriate JSON type as part of a name/value pair.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <param name="name">The UTF-16 encoded property name of the JSON object to be processed by <see cref="EncodeName"/> written as UTF-8.  If null (the default value), the expression for <paramref name="value"/> is used.</param>
    public static void WriteProperty<V>(
        Utf8JsonWriter writer,
        V value,
        JsonSerializerOptions options,
        [CallerArgumentExpression(nameof(value))] string name = null
    )
    {
        var serializer = GetSerializer<V>(options);
        try
        {
            writer.WritePropertyName(EncodeName(name, options));
            serializer(writer, value, options);
        }
        catch (Exception e)
        {
            var message = $"Error serializing property {name} ({value})";
            writer.WriteCommentValue(message);
            Plugin.Log.LogError(message);
            Plugin.Log.LogError(e);
        }
    }

    /// <summary>
    ///   Writes a <typeparamref name="V"/> value (as an appropriate
    ///   JSON type) as an element of a JSON array.
    /// </summary>
    /// <typeparam name="V">
    ///   The type of the value to convert to JSON.
    /// </typeparam>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">
    ///   The value to written as an appropriate JSON type as an element
    ///   of a JSON array.
    /// </param>
    /// <param name="options">
    ///   An object that specifies serialization options to use.
    /// </param>
    public static void WriteValue<V>(Utf8JsonWriter writer, V value, JsonSerializerOptions options)
    {
        var serializer = GetSerializer<V>(options);
        try
        {
            serializer(writer, value, options);
        }
        catch (Exception e)
        {
            var message = $"Error serializing value {value}";
            writer.WriteCommentValue(message);
            Plugin.Log.LogError(message);
            Plugin.Log.LogError(e);
        }
    }
}
