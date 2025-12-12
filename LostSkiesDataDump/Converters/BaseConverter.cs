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

namespace LostSkiesDataDump.Converters;

/// <summary>
/// Converts an object or value to or from JSON.
/// </summary>
/// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
public abstract partial class BaseConverter<T> : JsonConverter<T>
{
    /// <summary>
    /// <c>ID_KEY</c> is a JSON name whose corresponding value is the reference identifier of the object containing the name/value pair.
    /// </summary>
    public const string ID_KEY = "$id";

    /// <summary>
    /// <c>REFERENCE_KEY</c> is a JSON name whose corresponding value is a reference identifier used to resolve the object containing the name/value pair to another JSON object.
    /// </summary>
    public const string REFERENCE_KEY = "$ref";

    /// <summary>
    /// <c>TYPE_KEY</c> is a JSON name whose corresponding value indicates the type of the serialized object.
    /// </summary>
    public const string TYPE_KEY = "$type";

    /// <value>
    /// <c>Reference</c> indicates whether or not this converter should use references to avoid duplicating objects.  If true, this converter will use a reference when it encounters an object it has already serialized.  Otherwise, this converter will serialize objects it has encountered before.
    /// </value>
    public bool Reference { get; set; }

    /// <summary>
    /// This constructor initializes the converter with <see cref="Reference"/> set to true.
    /// </summary>
    public BaseConverter()
        : this(true) { }

    /// <summary>
    /// Initializes the converter.
    /// </summary>
    /// <param name="reference">Whether or not the new converter should use references.  The value is assigned to <see cref="Reference"/>.</param>
    public BaseConverter(bool reference)
    {
        Reference = reference;
        _ = Plugin.ConverterWriteCounter.GetEntry(GetType());
    }

    /// <summary>
    /// Reads and converts the JSON to type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options"> An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="NotImplementedException">Will always be raised because deserialization has not been implemented.</exception>
    public override T Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Writes a specified value as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        Plugin.ConverterWriteCounter.Increment(GetType());
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }
        writer.WriteStartObject();
        try
        {
            if (!Reference || !WriteReference(writer, value, options))
            {
                writer.WriteString(TYPE_KEY, typeof(T).ToString());
                WriteObjectBody(writer, value, options);
            }
        }
        catch (Exception e)
        {
            var message = $"Error writing {value}";
            writer.WriteCommentValue(message);
            Plugin.Log.LogError(message);
            Plugin.Log.LogError(e);
        }
        writer.WriteEndObject();
    }

    /// <summary>
    /// Writes the key/value pairs of the specified value as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public abstract void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    );

    /// <summary>
    /// Attempts to write a specified value as a reference.
    /// <para>
    /// If there is a reference resolver and the value was previously written, writes a reference for the value and returns true.  If there is a reference resolver, but the value was not written previously, assigns the value a reference identifier using the resolver, writes the id, and returns false.  Otherwise, returns false without writing anything.
    /// </para>
    /// </summary>
    /// <seealso cref="REFERENCE_KEY"/>
    /// <seealso cref="ID_KEY"/>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to attempt to write as a reference.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>True if the value was written as a reference; otherwise, false.</returns>
    public static bool WriteReference(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
            return false;
        if (options.ReferenceHandler?.CreateResolver() is not ReferenceResolver resolver)
            return false;
        var reference = resolver.GetReference(value, out bool alreadyExists);
        if (reference is null)
            return false;
        if (alreadyExists)
            writer.WriteString(REFERENCE_KEY, reference);
        else
            writer.WriteString(ID_KEY, reference);
        return alreadyExists;
    }
}
