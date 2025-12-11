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
public class ListConverterFactory : BaseConverterFactory, IConverterDefault<ListConverterFactory>
{
    public static ListConverterFactory Default { get; } = new();

    internal class ListConverter<TType, TItem>(bool reference, JsonSerializerOptions options)
        : FactoryConverter<TType>(reference)
        where TType : icg.List<TItem>
    {
        private readonly Serializer<TItem> _valueConverter = GetSerializer<TItem>(options);

        public override void WriteObjectBody(
            Utf8JsonWriter writer,
            TType value,
            JsonSerializerOptions options
        )
        {
            WriteArray(writer, GetItems(value), options, _valueConverter, "value");
        }

        public static IEnumerable<TItem> GetItems(TType entries)
        {
            foreach (var entry in entries)
                yield return entry;
        }
    }

    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType
        && typeToConvert.GetGenericTypeDefinition() == typeof(icg.List<>);

    internal override JsonConverter CreateNewConverter(
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        Type[] typeArguments = typeToConvert.GetGenericArguments();
        Type itemType = typeArguments[0];
        JsonConverter converter = (JsonConverter)
            Activator.CreateInstance(
                typeof(ListConverter<,>).MakeGenericType([typeToConvert, itemType]),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: [Reference, options],
                culture: null
            );
        return converter;
    }
}
