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
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using icg = Il2CppSystem.Collections.Generic;

namespace LostSkiesDataDump.Converters.Il2CppSystem.Collections.Generic;

[RequiresPreviewFeatures]
public class DictionaryConverterFactory
    : BaseConverterFactory,
        IConverterDefault<DictionaryConverterFactory>
{
    public static DictionaryConverterFactory Default { get; } = new();

    internal class DictionaryConverter<TType, TKey, TValue>(
        bool reference,
        JsonSerializerOptions options
    ) : FactoryConverter<TType>(reference)
        where TType : icg.Dictionary<TKey, TValue>
    {
        private readonly Serializer<TKey> _keyConverter = GetSerializer<TKey>(options);
        private readonly Serializer<TValue> _valueConverter = GetSerializer<TValue>(options);

        public override void WriteObjectBody(
            Utf8JsonWriter writer,
            TType value,
            JsonSerializerOptions options
        )
        {
            WriteArray(writer, GetKeyValuePairs(value), options, WriteEntry, "__entries");
        }

        public static IEnumerable<icg.KeyValuePair<TKey, TValue>> GetKeyValuePairs(TType entries)
        {
            foreach (var entry in entries)
                yield return entry;
        }

        public void WriteEntry(
            Utf8JsonWriter writer,
            icg.KeyValuePair<TKey, TValue> entry,
            JsonSerializerOptions options
        )
        {
            try
            {
                writer.WriteStartObject();
                writer.WritePropertyName(EncodeName("Key", options));
                _keyConverter(writer, entry.Key, options);
                writer.WritePropertyName(EncodeName("Value", options));
                _valueConverter(writer, entry.Value, options);
                writer.WriteEndObject();
            }
            catch (Exception e)
            {
                var message = $"Error serializing dictionary entry {entry}";
                writer.WriteCommentValue(message);
                Plugin.Log.LogError(message);
                Plugin.Log.LogError(e);
            }
        }
    }

    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType
        && typeToConvert.GetGenericTypeDefinition() == typeof(icg.Dictionary<,>);

    internal override JsonConverter CreateNewConverter(
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        Type[] typeArguments = typeToConvert.GetGenericArguments();
        Type keyType = typeArguments[0];
        Type valueType = typeArguments[1];
        JsonConverter converter = (JsonConverter)
            Activator.CreateInstance(
                typeof(DictionaryConverter<,,>).MakeGenericType(
                    [typeToConvert, keyType, valueType]
                ),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: [Reference, options],
                culture: null
            );
        return converter;
    }
}
