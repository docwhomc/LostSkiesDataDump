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
using System.Linq;
using System.Runtime.Versioning;
using System.Text.Json;
using Il2CppSystem.Linq;
using LostSkiesDataDump.Converters.WildSkies.Gameplay.Crafting;
using LostSkiesDataDump.Utilities;
using WildSkies.Weapon;

namespace LostSkiesDataDump.Converters.WildSkies.Weapon;

[RequiresPreviewFeatures]
public class WeaponLevelsConverter<T>
    : ItemLevelsConverter<T>,
        IConverterDefault<WeaponLevelsConverter<WeaponLevels>>
    where T : WeaponLevels
{
    public static new WeaponLevelsConverter<WeaponLevels> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        writer.WriteStartObject(EncodeName(nameof(value.GetWeaponProfile), options));
        // Sort levels by their integer value before calling `GetWeaponProfile(ItemLevel)`
        var levels = value.Levels.ToSystemEnumerable().Select(o => o.Level).OrderBy(o => o);
        foreach (var level in levels)
        {
            WeaponProfile weaponProfile = null;
            try
            {
                weaponProfile = value.GetWeaponProfile(level);
            }
            catch (Exception e)
            {
                var message = $"Error getting weapon profile for level {level}";
                writer.WriteCommentValue(message);
                Plugin.Log.LogError(message);
                Plugin.Log.LogDebug(e);
            }
            if (weaponProfile is not null)
                WriteProperty(writer, weaponProfile, options, level.ToString());
            else
            {
                writer.WritePropertyName(EncodeName(level.ToString(), options));
                writer.WriteNullValue();
            }
        }
        writer.WriteEndObject();
    }
}
