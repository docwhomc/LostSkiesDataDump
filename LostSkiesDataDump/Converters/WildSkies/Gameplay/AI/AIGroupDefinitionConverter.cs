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
public class AIGroupDefinitionConverter<T>
    : ScriptableObjectConverter<T>,
        IConverterDefault<AIGroupDefinitionConverter<AIGroupDefinition>>
    where T : AIGroupDefinition
{
    public static new AIGroupDefinitionConverter<AIGroupDefinition> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value.ID, options);
        WriteProperty(writer, value.UniqueID, options);
        WriteProperty(writer, value.AIGroupName, options);
        WriteProperty(writer, value.ExclusionRadius, options);
        WriteProperty(writer, value.DontSpawnNearPlayer, options);
        WriteProperty(writer, value.SpawnEntitiesOnNavMesh, options);
        WriteProperty(writer, value.SpawnInClouds, options);
        WriteProperty(writer, value.SpawnSkyFormation, options);
        WriteProperty(writer, value.MaxTotalEntities, options);
        WriteProperty(writer, value.SpawnRadius, options);
        WriteProperty(writer, value.SpawnHeightOffset, options);
        WriteProperty(writer, value.SpawnHeightNegativeOffset, options);
        WriteProperty(writer, value.SpawnDifficultyMatrix, options);
        WriteProperty(writer, value.GroupEntities, options);
        WriteProperty(writer, value.SpawnerType, options);
    }
}
