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

using System.Text.Json;
using LostSkiesDataDump.Converters.WildSkies.Gameplay.Items;
using WildSkies.Weapon;

namespace LostSkiesDataDump.Converters.WildSkies.Weapon;

public class WeaponProfileConverter<T> : ItemProfileConverter<T>
    where T : WeaponProfile
{
    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value.WeaponLevel, options);
        WriteProperty(writer, value.AmmoType, options);
        WriteProperty(writer, value.InputFireType, options);
        WriteProperty(writer, value.DamageMethod, options);
        WriteProperty(writer, value.DamageType, options);
        // TODO: `public unsafe LayerMask CollisionLayerMask`
        WriteProperty(writer, value.Damage, options);
        WriteProperty(writer, value.MaxDamage, options);
        WriteProperty(writer, value.RPM, options);
        WriteProperty(writer, value.MaxRPM, options);
        WriteProperty(writer, value.ClipSize, options);
        WriteProperty(writer, value.MaxClipSize, options);
        WriteProperty(writer, value.ReloadTimeInSeconds, options);
        WriteProperty(writer, value.MaxReloadTimeInSeconds, options);
        WriteProperty(writer, value.MaxRange, options);
        WriteProperty(writer, value.MaximalMaxRange, options);
        WriteProperty(writer, value.BackToIdleTime, options);
        WriteProperty(writer, value.BurstCount, options);
        WriteProperty(writer, value.BurstRate, options);
        WriteProperty(writer, value.PalletCount, options);
        WriteProperty(writer, value.IsMobileWeapon, options);
        WriteProperty(writer, value.Spread, options);
        WriteProperty(writer, value.SpreadMoving, options);
        WriteProperty(writer, value.SpreadADS, options);
        WriteProperty(writer, value.SpreadADSMoving, options);
        WriteProperty(writer, value.SpreadAddedPerShot, options);
        WriteProperty(writer, value.MaxShootingSpread, options);
        WriteProperty(writer, value.SpreadChangeSpeed, options);
        WriteProperty(writer, value.SpreadResetTime, options);
        WriteProperty(writer, value.MaxSpreadCalculated, options);
        // TODO: `public unsafe DynamikaCameraRecoil.RecoilData Recoil`
        WriteProperty(writer, value.CameraImpulseStrength, options);
        // TODO: `public unsafe ImpulseSpring ImpulseSpringSettings`
        WriteProperty(writer, value.HasDamageFalloff, options);
        WriteProperty(writer, value.DamageAtFurthestDistance, options);
        WriteProperty(writer, value.DistanceFallOffStart, options);
        WriteProperty(writer, value.DistanceFallOffEnd, options);
        WriteProperty(writer, value.KickbackImpulse, options);
        WriteProperty(writer, value.MovementPenaltyWhileFiringOrAds, options);
        WriteProperty(writer, value.MovementPenaltyWhileEquipped, options);
        WriteProperty(writer, value.FovChangeOnAds, options);
        WriteProperty(writer, value.AdsSensitivityModifier, options);
        WriteProperty(writer, value.HitPhysicsImpulse, options);
        WriteProperty(writer, value.ProjectileTravelSpeed, options);
        WriteProperty(writer, value.ProjectileDropAmount, options);
        WriteProperty(writer, value.ProjectileDropStartDistance, options);
        WriteProperty(writer, value._damageString, options);
        WriteProperty(writer, value._rpmString, options);
        WriteProperty(writer, value._clipSizeString, options);
        WriteProperty(writer, value._reloadTimeString, options);
        WriteProperty(writer, value._maxRangeString, options);
        WriteProperty(writer, value.HasBurst, options);
        WriteProperty(writer, value.IsShotgun, options);
        // TODO: `public unsafe LocalizedString GetLocalizedStringFromVariableName([DefaultParameterValue(null)] string variableName)`
        // TODO: `public unsafe override List<Il2CppSystem.ValueTuple<LocalizedString, string, float, float>> GetItemParameters()`
        // TODO: `public unsafe StatData GetStatData([DefaultParameterValue(null)] string statVariableName)`
    }
}
