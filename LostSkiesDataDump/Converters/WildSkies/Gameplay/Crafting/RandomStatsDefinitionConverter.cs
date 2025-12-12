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
using WildSkies.Gameplay.Crafting;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Crafting;

[RequiresPreviewFeatures]
public class RandomStatsDefinitionConverter<T>
    : BaseConverter<T>,
        IConverterDefault<RandomStatsDefinitionConverter<RandomStatsDefinition>>
    where T : RandomStatsDefinition
{
    public static RandomStatsDefinitionConverter<RandomStatsDefinition> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        // try
        // {
        //     Plugin.Log.LogDebug("> RandomStatsDefinition.SelectionList >");
        //     // WriteArray(writer, value.SelectionList.ToSystemEnumerable(), options);
        //     var index = 0;
        //     foreach (var element in value.SelectionList.ToSystemEnumerable())
        //     {
        //         Plugin.Log.LogDebug($">> RandomStatsDefinition.SelectionList[{index}] >>");
        //         try
        //         {
        //             WriteValue(writer, element, options);
        //         }
        //         catch (Exception e)
        //         {
        //             var message = $"Error serializing RandomStatsDefinition.SelectionList[{index}]: {element}";
        //             writer.WriteCommentValue(message);
        //             Plugin.Log.LogError(message);
        //             Plugin.Log.LogError(e);
        //         }
        //         Plugin.Log.LogDebug($"<< RandomStatsDefinition.SelectionList[{index}] <<");
        //         index++;
        //     }
        //     Plugin.Log.LogDebug("< RandomStatsDefinition.SelectionList <");
        // }
        // catch (Exception e)
        // {
        //     var message = $"Error serializing RandomStatsDefinition.SelectionList: {value.SelectionList}";
        //     writer.WriteCommentValue(message);
        //     Plugin.Log.LogError(message);
        //     Plugin.Log.LogError(e);
        // }
        WriteProperty(writer, value.NumSelections, options);
        WriteProperty(
            writer,
            value.NumSelectionWeightings,
            options,
            nameof(value.NumSelectionWeightings)
        );
        WriteProperty(writer, value.PointRange, options);
        // TODO: `public unsafe static int MinStatPoints`
        // TODO: `public unsafe static StatsSelectionAlgorithm Algorithm`
        WriteProperty(writer, value.IsValid(), options);
        // TODO: `public unsafe List<RandomStat> GetRandomStats(int variationSeed = -1)`
    }
}
