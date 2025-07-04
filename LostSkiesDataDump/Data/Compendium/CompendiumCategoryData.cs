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

using System.Collections.Generic;

namespace LostSkiesDataDump.Data.Compendium;

public class CompendiumCategoryData(CompendiumCategory compendiumCategory) : BaseData
{
    private readonly CompendiumCategory compendiumCategory = compendiumCategory;

    public override string GetIdentifier()
    {
        return Id;
    }

    public string Id => compendiumCategory.Id;
    public string Name => PreSerialize(compendiumCategory.Name);
    // TODO: Icon
    public bool IsMainCategory => compendiumCategory.IsMainCategory;
    public int PreferredIndex => compendiumCategory.PreferredIndex;

    public List<string> SubCategories
    {
        get
        {
            List<string> subCategories = [];
            foreach (CompendiumCategory subCategory in compendiumCategory.SubCategories)
            {
                subCategories.Add(subCategory.Id);
            }
            return subCategories;
        }
    }
}
