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
public class CraftableItemListEntryConverter<T>
    : MonoBehaviourConverter<T>,
        IConverterDefault<CraftableItemListEntryConverter<CraftableItemListEntry>>
    where T : CraftableItemListEntry
{
    public static new CraftableItemListEntryConverter<CraftableItemListEntry> Default { get; } =
        new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value._associatedSchematic, options);
        WriteProperty(writer, value._associatedItem, options);
        // public unsafe ScrollToOnSelect _scrollToOnSelect
        // public unsafe TextMeshProUGUI _name
        // public unsafe TextMeshProUGUI _schematicLevel
        // public unsafe Image _itemIcon
        WriteProperty(writer, value._newSchematicIcon, options);
        WriteProperty(writer, value._shipyardBuildIcon, options);
        WriteProperty(writer, value._fadeOutAlpha, options);
        // public unsafe Color _defaultColor
        // public unsafe Color _selectedColor
        // public unsafe Color _defaultFontColor
        // public unsafe Color _unableToCraftFontColor
        // public unsafe UiColourData _uiColourData
        // public unsafe Image _rarityGradient
        WriteProperty(writer, value._uncommonImage, options);
        WriteProperty(writer, value._rareImage, options);
        WriteProperty(writer, value._epicImage, options);
        WriteProperty(writer, value._levelString, options);
        WriteProperty(writer, value._outputItemName, options);
        WriteProperty(writer, value._itemLevel, options);
        // public unsafe Button _button
        // public unsafe CanvasGroup _fadeVfxCanvasGroup
        WriteProperty(writer, value._isShipyardItem, options);
        WriteProperty(writer, value._isInsideShipyard, options);
        WriteProperty(writer, value._haveAllResources, options);
        // public unsafe Button Button
        WriteProperty(writer, value.ItemLevel, options);
        WriteProperty(writer, value.AssociatedSchematic, options);
        WriteProperty(writer, value.AssociatedItem, options);
        WriteProperty(writer, value.CanCraftItem, options);
    }
}
