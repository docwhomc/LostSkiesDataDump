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
using System.Text.Json.Serialization;
using LostSkiesDataDump.Converters.Interfaces;
using WildSkies.Gameplay.Items;

namespace LostSkiesDataDump.Converters.WildSkies.Gameplay.Items;

[RequiresPreviewFeatures]
public class ItemCustomisationComponentConverter<T>
    : BaseItemComponentConverter<T>,
        IConverterDefault<ItemCustomisationComponentConverter<T>>
    where T : ItemCustomisationComponent
{
    public static new JsonConverter Default { get; } =
        new ItemCustomisationComponentConverter<ItemCustomisationComponent>();

    // static ItemCustomisationComponentConverter()
    // {
    //     SortedConverterSet.Default.Add(
    //         new ItemCustomisationComponentConverter<ItemCustomisationComponent>()
    //     );
    // }

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        // TODO: `public new unsafe static ItemTypes ClassItemType`
        WriteProperty(writer, value.CustomizationType, options);
        // TODO: `public unsafe CustomisationItem FemaleDynamikaCustomisation`
        // TODO: `public unsafe CustomisationItem MaleDynamikaCustomisation`
        // TODO: `public unsafe ClothingStats ClothingStats`
        // TODO: `public unsafe Il2CppReferenceArray<CustomColorScheme> OutfitColorSchemes`
        // TODO: `public unsafe override ItemTypes ItemType`
    }
}
