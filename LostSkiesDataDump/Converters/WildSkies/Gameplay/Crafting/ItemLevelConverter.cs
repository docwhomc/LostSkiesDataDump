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

using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using LostSkiesDataDump.Converters.Interfaces;
using WildSkies.Gameplay.Crafting;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Crafting;

[RequiresPreviewFeatures]
public class ItemLevelConverter<T> : BaseConverter<T>, IConverterDefault<ItemLevelConverter<T>>
    where T : ItemLevel
{
    public static JsonConverter Default { get; } = new ItemLevelConverter<ItemLevel>();

    // static ItemLevelConverter()
    // {
    //     SortedConverterSet.Default.Add(new ItemLevelConverter<ItemLevel>());
    // }

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.Level, options);
        WriteProperty(writer, value.RequiredXpAmount, options);
        WriteProperty(writer, value.Profile, options);
    }
}
