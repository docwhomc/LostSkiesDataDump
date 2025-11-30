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
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using Cysharp.Threading.Tasks.Linq;
using LostSkiesDataDump.Converters.Interfaces;

namespace LostSkiesDataDump.Converters;

[RequiresPreviewFeatures]
public class SortedConverterSet(IComparer<JsonConverter> comparer)
    : SortedSet<JsonConverter>(comparer)
{
    static readonly Type IConverterDefaultType = typeof(IConverterDefault<>);
    public static SortedConverterSet Default { get; } = new(ConverterComparer.Default);

    internal static void AddToDefault(JsonConverter converter)
    {
        if (!Default.Add(converter))
            Plugin.Log.LogError($"unable to add {converter} to {Default}");
    }

    static SortedConverterSet()
    {
        Plugin.Log.LogInfo($"initializing default {nameof(SortedConverterSet)}");
        foreach (JsonConverter converter in GetDefaultConverters())
            AddToDefault(converter);
        AddToDefault(new JsonStringEnumConverter());
    }

    internal static IEnumerable<JsonConverter> GetDefaultConverters()
    {
        foreach (var converterType in GetConverterTypes())
            if (GetDefaultConverter(converterType, out JsonConverter converter))
                yield return converter;
    }

    public static IEnumerable<Type> GetConverterTypes(Assembly assembly = null) =>
        (assembly ?? Assembly.GetExecutingAssembly())
            .GetTypes()
            .Where(type =>
                type.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == IConverterDefaultType
                    )
            );

    private static bool GetDefaultConverter(Type converterType, out JsonConverter instance)
    {
        Type[] typeParameters = converterType.GetGenericArguments();
        switch (typeParameters.Length)
        {
            case 0:
                break;
            case 1:
                Type typeParameter = typeParameters[0];
                if (typeParameter.IsGenericParameter)
                {
                    Type[] constraints = typeParameter.GetGenericParameterConstraints();
                    if (constraints.Length != 1)
                    {
                        Plugin.Log.LogError(
                            $"{converterType} has a type parameter with {constraints.Length} constraints, expected 1"
                        );
                        instance = null;
                        return false;
                    }
                    converterType = converterType.MakeGenericType([constraints[0]]);
                }
                break;
            default:
                Plugin.Log.LogError(
                    $"{converterType} has {typeParameters.Length} type parameters, expected 0 or 1"
                );
                instance = null;
                return false;
        }
        PropertyInfo defaultProperty = converterType.GetProperty(
            nameof(IConverterDefault<>.Default)
        );
        try
        {
            instance = (JsonConverter)defaultProperty.GetValue(converterType);
        }
        catch (Exception e)
        {
            Plugin.Log.LogError($"failed to get Default instance of {converterType}");
            Plugin.Log.LogError(e);
            instance = null;
            return false;
        }
        return true;
    }

    public class ConverterComparer(TypeComparer typeComparer = null) : Comparer<JsonConverter>
    {
        public static new readonly ConverterComparer Default = new();
        public TypeComparer TypeComparer = typeComparer ?? TypeComparer.Default;

        public override int Compare(JsonConverter x, JsonConverter y)
        {
            int compare;
            Type xType = x.GetType();
            Type yType = y.GetType();
            compare = TypeComparer.Compare(xType, yType);
            return compare;
        }
    }

    public class TypeComparer : Comparer<Type>
    {
        public static new readonly TypeComparer Default = new();

        internal Dictionary<Tuple<Type, Type>, int> Cache = [];

        public void ClearCache() => Cache.Clear();

        public override int Compare(Type x, Type y)
        {
            int compare;
            Tuple<Type, Type> key = new(x, y);
            bool notFound = !Cache.TryGetValue(key, out compare);
            if (notFound)
            {
                compare = SubclassCompare(x, y);
                Cache.TryAdd(key, compare);
            }
            return compare;
        }

        internal static int SubclassCompare(Type x, Type y)
        {
            Type[] xHierarchy = [.. GetClassHierarchy(x).Reverse()];
            Type[] yHierarchy = [.. GetClassHierarchy(y).Reverse()];
            int stop = Math.Min(xHierarchy.Length, yHierarchy.Length);
            for (int index = 0; index < stop; index++)
            {
                Type xBase = TryGetGeneric(xHierarchy[index]);
                Type yBase = TryGetGeneric(yHierarchy[index]);
                if (xBase != yBase)
                    return StringComparer.InvariantCulture.Compare(xBase.Name, yBase.Name);
            }
            return yHierarchy.Length.CompareTo(xHierarchy.Length);
        }

        internal static Type TryGetGeneric(Type type) =>
            type.IsGenericType ? type.GetGenericTypeDefinition() : type;

        internal static IEnumerable<Type> GetClassHierarchy(Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}
