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
using WildSkies.Service;

namespace LostSkiesDataDump.Data.Compendium;

public class CompendiumData(ICompendiumService compendiumService)
{
    private readonly ICompendiumService _CompendiumService = compendiumService;

    public List<CompendiumCategoryData> Categories
    {
        get
        {
            List<CompendiumCategoryData> categories = [];
            foreach (CompendiumCategory category in _CompendiumService.Categories)
            {
                CompendiumCategoryData categoryData = new(category);
                categories.Add(categoryData);
            }
            return categories;
        }
    }

    public List<CompendiumEntryData> Entries
    {
        get
        {
            List<CompendiumEntryData> entries = [];
            foreach (CompendiumEntry entry in _CompendiumService.Entries)
            {
                CompendiumEntryData entryData = new(entry);
                entries.Add(entryData);
            }
            return entries;
        }
    }
}
