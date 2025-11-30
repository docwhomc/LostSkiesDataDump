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
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using LostSkiesDataDump.Converters.Interfaces;
using icg = Il2CppSystem.Collections.Generic;

namespace LostSkiesDataDump.Converters.Il2CppSystem.Collections.Generic;

[RequiresPreviewFeatures]
public class ListConverterFactory : BaseConverterFactory, IConverterDefault<ListConverterFactory>
{
    public static JsonConverter Default { get; } = new ListConverterFactory();

    // static ListConverterFactory()
    // {
    //     SortedConverterSet.Default.Add(new ListConverterFactory());
    // }

    internal class ListConverter<TType, TValue>(bool reference) : FactoryConverter<TType>(reference)
        where TType : icg.List<TValue>
    {
        public override void WriteObjectBody(
            Utf8JsonWriter writer,
            TType value,
            JsonSerializerOptions options
        )
        {
            WriteArray(writer, value, options);
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
        Type valueType = typeArguments[0];
        JsonConverter converter = (JsonConverter)
            Activator.CreateInstance(
                typeof(ListConverter<,>).MakeGenericType([typeToConvert, valueType]),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: [Reference],
                culture: null
            );
        return converter;
    }
}
