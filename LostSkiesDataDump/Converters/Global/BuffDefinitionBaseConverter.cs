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

namespace LostSkiesDataDump.Converters.Global;

[RequiresPreviewFeatures]
public class BuffDefinitionBaseConverter<T>
    : ScriptableObjectConverter<T>,
        IConverterDefault<BuffDefinitionBaseConverter<BuffDefinitionBase>>
    where T : BuffDefinitionBase
{
    public static new BuffDefinitionBaseConverter<BuffDefinitionBase> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value._NameRaw_k__BackingField, options);
        WriteProperty(writer, value._Name_k__BackingField, options);
        WriteProperty(writer, value._Description_k__BackingField, options);
        WriteProperty(writer, value._Duration_k__BackingField, options);
        WriteProperty(writer, value._TimeFormat_k__BackingField, options);
        WriteProperty(writer, value._Stacks_k__BackingField, options);
        WriteProperty(writer, value._StackFormat_k__BackingField, options);
        WriteProperty(writer, value._Timed_k__BackingField, options);
        WriteProperty(writer, value._DurationIsPerStack_k__BackingField, options);
        WriteProperty(writer, value._CustomPosition_k__BackingField, options);
        WriteProperty(writer, value._HigherStacksHaveShorterDuration_k__BackingField, options);
        WriteProperty(writer, value._CanBeAddedByDamage_k__BackingField, options);
        WriteProperty(writer, value._AddedByDamageType_k__BackingField, options);
        WriteProperty(writer, value._EquivalentShipPartBuff_k__BackingField, options);
        WriteProperty(writer, value._VfxType_k__BackingField, options);
        WriteProperty(writer, value._OverallEffect_k__BackingField, options);
        WriteProperty(writer, value._RemovesBuff_k__BackingField, options);
        WriteProperty(writer, value._DurationMultiplierPerStack_k__BackingField, options);
        WriteProperty(writer, value._CustomPrefab_k__BackingField, options);
        WriteProperty(writer, value._Icon_k__BackingField, options);
        WriteProperty(writer, value._BuffConditions_k__BackingField, options);
        WriteProperty(writer, value._BuffEffects_k__BackingField, options);
        WriteProperty(writer, value.NameRaw, options);
        WriteProperty(writer, value.Name, options);
        WriteProperty(writer, value.Description, options);
        WriteProperty(writer, value.Duration, options);
        WriteProperty(writer, value.TimeFormat, options);
        WriteProperty(writer, value.Stacks, options);
        WriteProperty(writer, value.StackFormat, options);
        WriteProperty(writer, value.Timed, options);
        WriteProperty(writer, value.DurationIsPerStack, options);
        WriteProperty(writer, value.CustomPosition, options);
        WriteProperty(writer, value.HigherStacksHaveShorterDuration, options);
        WriteProperty(writer, value.CanBeAddedByDamage, options);
        WriteProperty(writer, value.AddedByDamageType, options);
        WriteProperty(writer, value.EquivalentShipPartBuff, options);
        WriteProperty(writer, value.VfxType, options);
        WriteProperty(writer, value.OverallEffect, options);
        WriteProperty(writer, value.RemovesBuff, options);
        WriteProperty(writer, value.DurationMultiplierPerStack, options);
        WriteProperty(writer, value.CustomPrefab, options);
        WriteProperty(writer, value.Icon, options);
        WriteProperty(writer, value.BuffConditions, options);
        WriteProperty(writer, value.BuffEffects, options);
    }
}
