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

namespace LostSkiesDataDump.Utilities;

public class TypeSerializationCounter
{
    public readonly struct Key(Type type, Type serializedAsType)
    {
        public const string NULL = "Null";
        public readonly Type Type = type;
        public readonly Type SerializedAsType = serializedAsType;

        public override string ToString() =>
            Type == SerializedAsType
                ? Type?.GetPrettyName() ?? NULL
                : $"{Type?.GetPrettyName() ?? NULL} as {SerializedAsType?.GetPrettyName() ?? NULL}";
    }

    public class KeyComparer(IComparer<Type> typeComparer = null) : Comparer<Key>
    {
        public static new readonly KeyComparer Default = new();
        public readonly IComparer<Type> TypeComparer =
            typeComparer ?? Utilities.TypeComparer.Default;

        public override int Compare(Key x, Key y)
        {
            int value = TypeComparer.Compare(x.Type, y.Type);
            if (value != 0)
                return value;
            return TypeComparer.Compare(x.SerializedAsType, y.SerializedAsType);
        }
    }

    public Dictionary<Key, uint> Counts = [];

    public uint this[Key key]
    {
        get => Counts.TryGetValue(key, out uint count) ? count : 0;
        set => Counts[key] = value;
    }

    public uint this[Type type, Type serializedAsType]
    {
        get => this[new Key(type, serializedAsType)];
        set => this[new Key(type, serializedAsType)] = value;
    }

    public void Increment(Key key)
    {
        this[key]++;
    }

    public void Increment(Type type, Type serializedAsType) =>
        Increment(new Key(type, serializedAsType));

    public IEnumerable<string> MakeReport()
    {
        foreach (var item in Counts.OrderBy(item => item.Key, KeyComparer.Default))
        {
            yield return $"{item.Key}: {item.Value}";
        }
    }

    public void Clear()
    {
        Counts.Clear();
    }
}
