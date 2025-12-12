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
using LostSkiesDataDump.Extensions;

namespace LostSkiesDataDump.Converters;

public class TypeComparer : Comparer<Type>
{
    public static new readonly TypeComparer Default = new();

    internal Dictionary<Tuple<Type, Type>, int> Cache = [];

    public void ClearCache() => Cache.Clear();

    public override int Compare(Type x, Type y)
    {
        Tuple<Type, Type> key = new(x, y);
        bool notFound = !Cache.TryGetValue(key, out int compare);
        if (notFound)
        {
            compare = SubclassCompare(x, y);
            Cache.TryAdd(key, compare);
        }
        return compare;
    }

    internal static int SubclassCompare(Type x, Type y)
    {
        Type[] xHierarchy = [.. x.GetClassHierarchy().Reverse()];
        Type[] yHierarchy = [.. y.GetClassHierarchy().Reverse()];
        int stop = Math.Min(xHierarchy.Length, yHierarchy.Length);
        for (int index = 0; index < stop; index++)
        {
            Type xBase = xHierarchy[index].TryGetGeneric();
            Type yBase = yHierarchy[index].TryGetGeneric();
            if (xBase != yBase)
                return StringComparer.InvariantCulture.Compare(xBase.Name, yBase.Name);
        }
        return yHierarchy.Length.CompareTo(xHierarchy.Length);
    }
}
