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
using LostSkiesDataDump.Converters.UnityEngine;
using WildSkies.Gameplay.AI;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.AI;

[RequiresPreviewFeatures]
public class AIDefinitionConverter<T>
    : ScriptableObjectConverter<T>,
        IConverterDefault<AIDefinitionConverter<AIDefinition>>
    where T : AIDefinition
{
    public static new AIDefinitionConverter<AIDefinition> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value.ID, options);
        WriteProperty(writer, value.AIName, options);
        WriteProperty(writer, value.LocalisedName, options);
        WriteProperty(writer, value.AIType, options);
        WriteProperty(writer, value.IsAlpha, options);
        WriteProperty(writer, value.ExclusionRadius, options);
        WriteProperty(writer, value.DontSpawnNearPlayer, options);
        WriteProperty(writer, value.SpawnOnNavMesh, options);
        WriteProperty(writer, value.LocalVariant, options);
        WriteProperty(writer, value.SpawnDifficultyMatrix, options);
        WriteProperty(writer, value.LootContainers, options);
        WriteProperty(writer, value.Levels, options);
        WriteProperty(writer, value.ScanTime, options);
        WriteProperty(writer, value.LevelModifierMin, options);
        WriteProperty(writer, value.LevelModifierMax, options);
    }
}
