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

namespace LostSkiesDataDump.Wrappers.Compendium;

public class CompendiumServiceWrapper(ICompendiumService compendiumService) : BaseWrapper
{
    private readonly ICompendiumService _compendiumService = compendiumService;

    public override string GetIdentifier()
    {
        return CompendiumService.CompendiumPrefix;
    }

    public List<CompendiumCategoryWrapper> Categories
    {
        get
        {
            List<CompendiumCategoryWrapper> categories = [];
            foreach (CompendiumCategory category in _compendiumService.Categories)
            {
                CompendiumCategoryWrapper categoryWrapper = new(category);
                categories.Add(categoryWrapper);
            }
            return categories;
        }
    }

    public List<CompendiumEntryWrapper> Entries
    {
        get
        {
            List<CompendiumEntryWrapper> entries = [];
            foreach (CompendiumEntry entry in _compendiumService.Entries)
            {
                CompendiumEntryWrapper entryWrapper = new(entry);
                entries.Add(entryWrapper);
            }
            return entries;
        }
    }
}
