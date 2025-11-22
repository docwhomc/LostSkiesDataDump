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
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LostSkiesDataDump.Converters;

public abstract class BaseConverterFactory(bool reference = true) : JsonConverterFactory
{
    public bool Reference { get; set; } = reference;
    internal Dictionary<Type, JsonConverter> ConverterCache { get; set; } = [];

    internal abstract class FactoryConverter<TType>(bool reference)
        : BaseConverter<TType>(reference) { }

    internal abstract JsonConverter CreateNewConverter(
        Type typeToConvert,
        JsonSerializerOptions options
    );

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        bool success = ConverterCache.TryGetValue(typeToConvert, out JsonConverter converter);
        if (success)
            Plugin.Log.LogDebug(
                $"{this} retrieved converter {converter} for {typeToConvert} from cache"
            );
        else
        {
            Plugin.Log.LogDebug($"{this} doesn't have a converter cached for {typeToConvert}");
            converter = CreateNewConverter(typeToConvert, options);
            Plugin.Log.LogDebug($"{this} created converter {converter} for {typeToConvert}");
            TryCacheConverter(typeToConvert, converter);
        }
        return converter;
    }

    public void ClearConverterCache() => ConverterCache.Clear();

    private bool TryCacheConverter(Type typeToConvert, JsonConverter converter)
    {
        bool success = ConverterCache.TryAdd(typeToConvert, converter);
        if (success)
            Plugin.Log.LogDebug($"{this} cached converter {converter} for {typeToConvert}");
        else if (ConverterCache.ContainsKey(typeToConvert))
        {
            bool getSuccess = ConverterCache.TryGetValue(
                typeToConvert,
                out JsonConverter otherConverter
            );
            if (ReferenceEquals(converter, otherConverter))
                Plugin.Log.LogError(
                    $"{this} failed to cache converter {converter} for {typeToConvert} because it ({otherConverter}) is already cached"
                );
            else
                Plugin.Log.LogError(
                    $"{this} failed to cache converter {converter} for {typeToConvert} because another converter {otherConverter} is already cached"
                );
            Debug.Assert(getSuccess);
        }
        else
            Plugin.Log.LogError(
                $"{this} failed to cache converter {converter} for {typeToConvert} for unknown reason"
            );
        return success;
    }
}
