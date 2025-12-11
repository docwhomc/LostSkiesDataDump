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
using Bossa.Dynamika.Utilities;

namespace LostSkiesDataDump.Converters.Bossa.Dynamika.Utilities;

[RequiresPreviewFeatures]
public class UtilityItemConverter<T>
    : BaseConverter<T>,
        IConverterDefault<UtilityItemConverter<UtilityItem>>
    where T : UtilityItem
{
    public static UtilityItemConverter<UtilityItem> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        // TODO: `public unsafe DynamikaCharacter _Character_k__BackingField`
        WriteProperty(writer, value._utilityGroup, options);
        WriteProperty(writer, value._utilitySlot, options);
        WriteProperty(writer, value._spawnOutOfHierarchy, options);
        // TODO: `public unsafe HumanBodyBones _bone`
        WriteProperty(writer, value._slotLocalPosition, options);
        WriteProperty(writer, value._slotLocalRotation, options);
        WriteProperty(writer, value._overrideMovementModule, options);
        // TODO: `public unsafe MovementModule _movementModule`
        // TODO: `public unsafe Il2CppReferenceArray<IKTargetTransform> _ikTargetTransforms`
        WriteProperty(writer, value._hasCustomLocomotionWhenActive, options);
        WriteProperty(writer, value._locomotionTypeActive, options);
        WriteProperty(writer, value._hasCustomLocomotionWhenEquipped, options);
        WriteProperty(writer, value._locomotionTypeEquipped, options);
        WriteProperty(writer, value._EvaluateIKTargets_k__BackingField, options);
        WriteProperty(writer, value._isActivated, options);
        WriteProperty(writer, value._currentHealth, options);
        // TODO: `public unsafe DynamikaCharacter Character`
        WriteProperty(writer, value.OverrideMovementModule, options);
        // TODO: `public unsafe MovementModule MovementModule`
        WriteProperty(writer, value.UtilitySlot, options);
        WriteProperty(writer, value.UtilityGroup, options);
        WriteProperty(writer, value.HasCustomLocomotionWhenActive, options);
        WriteProperty(writer, value.LocomotionTypeActive, options);
        WriteProperty(writer, value.HasCustomLocomotionWhenEquipped, options);
        WriteProperty(writer, value.LocomotionTypeEquipped, options);
        // TODO: `public unsafe Il2CppReferenceArray<IKTargetTransform> IKTargetTransforms`
        WriteProperty(writer, value.SpawnOutOfHierarchy, options);
        WriteProperty(writer, value.Bone, options);
        WriteProperty(writer, value.IsActivated, options);
        WriteProperty(writer, value.EvaluateIKTargets, options);
    }
}
