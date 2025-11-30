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
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using LostSkiesDataDump.Converters.Interfaces;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using ics = Il2CppSystem;

namespace LostSkiesDataDump.Converters.UnityEngine.Localization;

[RequiresPreviewFeatures]
public class LocalizedStringConverter<T>
    : BaseConverter<T>,
        IConverterDefault<LocalizedStringConverter<T>>
    where T : LocalizedString
{
    public static JsonConverter Default { get; } = new LocalizedStringConverter<LocalizedString>();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        if (HasTableReference(value))
        {
            var initialLocale = value.LocaleOverride;
            try
            {
                var availableLocales = LocalizationSettings.AvailableLocales;
                foreach (var locale in availableLocales.Locales)
                {
                    var localeString = locale.Identifier.Code;
                    value.LocaleOverride = locale;
                    var stringValue = value.GetLocalizedString();
                    WriteProperty(writer, stringValue, options, localeString);
                }
            }
            catch (Exception e)
            {
                Plugin.Log.LogError(e);
            }
            value.LocaleOverride = initialLocale;
        }
        else
        {
            var message =
                "Unable to serialize LocalizedString that does not specify a table collection";
            writer.WriteCommentValue(message);
            Plugin.Log.LogDebug(message);
        }
    }

    public static bool HasTableReference(LocalizedString localizedString) =>
        localizedString.TableReference is not null
        && (
            localizedString.TableReference.TableCollectionName is not null
            || localizedString.TableReference.TableCollectionNameGuid != ics.Guid.Empty
        );
}
