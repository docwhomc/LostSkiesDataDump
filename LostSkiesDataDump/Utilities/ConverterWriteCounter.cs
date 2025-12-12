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
using System.Linq;
using System.Text.Json.Serialization;
using LibCpp2IL;

namespace LostSkiesDataDump.Utilities;

public class ConverterWriteCounter
{
    public enum SortBy
    {
        None,
        Name,
        Type,
    }

    public class Entry
    {
        private static readonly HashSet<Type> SkipTypes =
        [
            null,
            typeof(object),
            typeof(JsonConverter<>),
            typeof(JsonConverter),
            typeof(JsonConverterFactory),
        ];
        public readonly ConverterWriteCounter Report;
        public readonly Type Type;
        public readonly Type BaseType;
        public readonly string Name;
        private readonly List<Entry> Children = [];
        public uint DirectCount { get; private set; } = 0;
        public uint ChildCount { get; private set; } = 0;
        public Entry BaseEntry => BaseType != null ? Report.GetEntry(BaseType) : null;
        public bool HasChildren => Children.Count != 0;

        internal Entry(ConverterWriteCounter report, Type type)
        {
            Report = report;
            Type = type.TryGetGeneric();
            Type baseType = Type.BaseType;
            if (SkipTypes.Contains(baseType))
            {
                BaseType = null;
                Report.RootEntries.Add(this);
            }
            else
            {
                BaseType = baseType.TryGetGeneric();
                BaseEntry?.Children.Add(this);
            }
            Name = type.GetPrettyName();
        }

        public void DirectIncrement()
        {
            DirectCount++;
            BaseEntry?.BaseIncrement();
        }

        private void BaseIncrement()
        {
            ChildCount++;
            BaseEntry?.BaseIncrement();
        }

        public static string GetName(Entry entry) => entry.Name;

        public static Type GetType(Entry entry) => entry.Type;

        public static IEnumerable<Entry> Order(IEnumerable<Entry> entries, SortBy sortBy) =>
            sortBy switch
            {
                SortBy.None => entries,
                SortBy.Name => entries.OrderBy(GetName),
                SortBy.Type => entries.OrderBy(GetType, TypeComparer.Default),
                _ => throw new ArgumentException($"invalid value for sortBy: {sortBy}"),
            };

        public static IEnumerable<Entry> GetChildrenRecursive(
            IEnumerable<Entry> entries,
            SortBy sortBy = SortBy.None
        )
        {
            foreach (Entry entry in entries)
            {
                yield return entry;
                foreach (Entry childEntry in GetChildrenRecursive(entry.Children, sortBy))
                    yield return childEntry;
            }
        }

        public static IEnumerable<Tuple<Entry, uint>> GetChildrenWithLevels(
            IEnumerable<Entry> entries,
            SortBy sortBy = SortBy.None,
            uint level = 0
        )
        {
            foreach (Entry entry in entries)
            {
                yield return new(entry, level);
                IEnumerable<Tuple<Entry, uint>> children = GetChildrenWithLevels(
                    entry.Children,
                    sortBy,
                    level + 1
                );
                foreach (Tuple<Entry, uint> child in children)
                    yield return child;
            }
        }

        public IEnumerable<Entry> GetChildren(SortBy sortBy = SortBy.None) =>
            Order(Children, sortBy);

        public IEnumerable<Entry> GetChildrenRecursive(SortBy sortBy = SortBy.None) =>
            GetChildrenRecursive(Children, sortBy);

        public IEnumerable<Tuple<Entry, uint>> GetChildrenWithLevels(
            SortBy sortBy = SortBy.None,
            uint level = 0
        ) => GetChildrenWithLevels(Children, sortBy, level);

        public string ReportText()
        {
            if (HasChildren)
                return $"{Name}: {DirectCount} [{ChildCount}]";
            Debug.Assert(
                ChildCount == 0,
                $"{ChildCount} calls via children, but no children recorded"
            );
            return $"{Name}: {DirectCount}";
        }
    }

    private readonly Dictionary<Type, Entry> Entries = [];
    private readonly List<Entry> RootEntries = [];

    private Entry AddNewEntry(Type type)
    {
        Entry entry = new(this, type);
        Entries.Add(type, entry);
        return entry;
    }

    public Entry GetEntry(Type type)
    {
        type = type.TryGetGeneric();
        return Entries.TryGetValue(type, out Entry entry) ? entry : AddNewEntry(type);
    }

    public void Clear()
    {
        Entries.Clear();
        RootEntries.Clear();
    }

    public void Increment(Type type) => GetEntry(type).DirectIncrement();

    public IEnumerable<Entry> GetEntries(SortBy sortBy = SortBy.None) =>
        Entry.Order(Entries.Values, sortBy);

    public IEnumerable<Entry> GetRootEntries(SortBy sortBy = SortBy.None) =>
        Entry.Order(RootEntries, sortBy);

    public IEnumerable<Entry> GetEntryHierarchy(SortBy sortBy = SortBy.None) =>
        Entry.GetChildrenRecursive(RootEntries, sortBy);

    public IEnumerable<string> GenerateReport(
        SortBy sortBy = SortBy.None,
        bool hierarchy = false,
        string indent = "  "
    )
    {
        if (hierarchy)
        {
            IEnumerable<Tuple<Entry, uint>> entryLevelPairs = Entry.GetChildrenWithLevels(
                RootEntries,
                sortBy
            );
            foreach (Tuple<Entry, uint> entryLevelPair in entryLevelPairs)
                yield return $"{indent.Repeat((int)entryLevelPair.Item2)}{entryLevelPair.Item1.ReportText()}";
        }
        else
            foreach (Entry entry in GetEntries(sortBy))
                yield return entry.ReportText();
    }
}
