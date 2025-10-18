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

public abstract partial class BaseConverter<T>(bool reference) : JsonConverter<T>
{
    public const string ID_KEY = "$id";
    public const string REFERENCE_KEY = "$ref";
    public const string TYPE_KEY = "$type";
    public bool Reference { get; set; } = reference;

    public BaseConverter()
        : this(true) { }

    public override T Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
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

    public abstract void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    );

    // Returns true if REFERENCE_KEY is written, false if ID ID_KEY is written or if there is no reference.
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
