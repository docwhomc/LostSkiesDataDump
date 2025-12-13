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
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.Json;
using LostSkiesDataDump.Converters.Bossa.Dynamika.Utilities;
using UnityEngine;
using Utilities.Weapons;

namespace LostSkiesDataDump.Converters.Utilities.Weapons;

[RequiresPreviewFeatures]
public class WeaponBaseConverter<T>
    : UtilityItemConverter<T>,
        IConverterDefault<WeaponBaseConverter<WeaponBase>>
    where T : WeaponBase
{
    public static new WeaponBaseConverter<WeaponBase> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        // TODO: `public unsafe static string SchematicPrefix`
        // TODO: `public unsafe Il2CppSystem.Action OnShoot`
        // TODO: `public unsafe ShotResponseEvent OnShotResponse`
        WriteProperty(writer, value._CanShoot_k__BackingField, options);
        WriteProperty(writer, value._CanActivate_k__BackingField, options);
        WriteProperty(writer, value._hasLeftHandPose, options);
        WriteProperty(writer, value._leftHandPose, options);
        WriteProperty(writer, value._hasRightHandPose, options);
        WriteProperty(writer, value._rightHandPose, options);
        WriteProperty(writer, value._itemWeaponComponent, options);
        WriteProperty(writer, value._weaponLevels, options);
        WriteProperty(writer, value._weaponProfile, options);
        WriteProperty(writer, value._bulletSource, options);
        WriteProperty(writer, value._recoilTypeString, options);
        WriteProperty(writer, value._recoilStrength, options);
        WriteProperty(writer, value._canAim, options);
        WriteProperty(writer, value._reticuleType, options);
        WriteProperty(writer, value._doAimIk, options);
        WriteProperty(writer, value._ikAimSpeed, options);
        WriteProperty(writer, value._aimLookAheadDistance, options);
        // TODO: `public unsafe PlayersService PlayersService`
        // TODO: `public unsafe IItemService _itemService`
        // TODO: `public unsafe NetworkFxService _networkFxService`
        // TODO: `public unsafe ColliderLookupService _colliderLookupService`
        // TODO: `public unsafe ISchematicLevelService _schematicLevelService`
        // TODO: `public unsafe Animator _modelAnimator`
        WriteProperty(writer, value._currentFireTimer, options);
        WriteProperty(writer, value._activationTimer, options);
        WriteProperty(writer, value._burstCooldownTimer, options);
        WriteProperty(writer, value._spreadLerpT, options);
        WriteProperty(writer, value._currentBurstShot, options);
        WriteProperty(writer, value._itemID, options);
        WriteProperty(writer, value._aimPosition, options);
        WriteProperty(writer, value._shootRequest, options);
        WriteProperty(writer, value._isAiming, options);
        WriteProperty(writer, value._firstShot, options);
        WriteProperty(writer, value._wasFiring, options);
        WriteProperty(writer, value._burstCoolDown, options);
        WriteProperty(writer, value._debugNoSpread, options);
        WriteProperty(writer, value._shootingSpreadAdded, options);
        WriteProperty(writer, value._schematicLevel, options);
        WriteProperty(writer, value._firingHeldStarted, options);
        WriteProperty(writer, value._currentSpread, options);
        WriteProperty(writer, value._weaponWasDestroyed, options);
        // TODO: `public unsafe static float SecondsPerMinute`
        WriteProperty(writer, value.CanShoot, options);
        WriteProperty(writer, value.CanActivate, options);
        WriteProperty(writer, value.CollisionLayerMask, options);
        WriteProperty(writer, value.IsAiming, options);
        TryWriteProperty<float?>(writer, value, options, nameof(value.FovChangeOnAim));
        TryWriteProperty<Vector2?>(writer, value, options, nameof(value.AimSensitivityModifier));
        TryWriteProperty<string>(writer, value, options, nameof(value.RecoilTypeString));
        WriteProperty(writer, value.RecoilStrength, options);
        // TODO: `public unsafe DynamikaCameraRecoil.RecoilData RecoilData`
        TryWriteProperty<bool?>(writer, value, options, nameof(value.IsBurstOnCoolDown));
        WriteProperty(writer, value.ItemID, options);
        WriteProperty(writer, value.WeaponProfile, options);
        WriteProperty(writer, value.DebugNoSpread, options);
        WriteProperty(writer, value.ReticuleType, options);
        // TODO: `public unsafe Animator ModelAnimator`
        WriteProperty(writer, value.BulletSource, options);
        WriteProperty(writer, value.SchematicId, options);
        WriteProperty(writer, value.CurrentSpread, options);
        // TODO: `public unsafe virtual void SetUtilityLevel([DefaultParameterValue(null)] int level)`
        // TODO: `public unsafe Vector3 GetDirectionBySpread([DefaultParameterValue(null)] Vector3 direction, [DefaultParameterValue(null)] Vector3 currentSpread)`
        // TODO: `public unsafe float GetWeaponStatValue([DefaultParameterValue(null)] string variableName)`
    }

    public void TryWriteProperty<V>(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options,
        string name
    )
    {
        var propertyValue = TryGetProperty<V>(value, name);
        WriteProperty(writer, propertyValue, options, name);
    }

    public V TryGetProperty<V>(T value, string name)
    {
        Type type = typeof(T);
        PropertyInfo propertyInfo = null;
        try
        {
            propertyInfo = type.GetProperty(name);
        }
        catch (Exception e)
        {
            Plugin.Log.LogError($"{e.GetType()} getting PropertyInfo of {value}.{name}");
            Plugin.Log.LogDebug(e);
        }
        object propertyValue = null;
        try
        {
            propertyValue = propertyInfo?.GetValue(value);
        }
        catch (Exception e)
        {
            Plugin.Log.LogWarning($"{e.GetType()} getting value of {value}.{name}");
            Plugin.Log.LogDebug(e);
        }
        V castValue = default;
        try
        {
            castValue = (V)propertyValue;
        }
        catch (Exception e)
        {
            Plugin.Log.LogError(
                $"{e.GetType()} casting value of {value}.{name} from {propertyValue} to {typeof(V)}"
            );
            Plugin.Log.LogDebug(e);
        }
        return castValue;
    }
}
