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
public class ItemLevelsConverter<T> : BaseConverter<T>, IConverterDefault<ItemLevelsConverter<T>>
    where T : ItemLevels
{
    public static JsonConverter Default { get; } = new ItemLevelsConverter<ItemLevels>();

    // static ItemLevelsConverter()
    // {
    //     SortedConverterSet.Default.Add(new ItemLevelsConverter<ItemLevels>());
    // }

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteArray(writer, value.Levels.ToSystemEnumerable(), options);
        WriteProperty(writer, value.LevelsCount, options);
    }
}
