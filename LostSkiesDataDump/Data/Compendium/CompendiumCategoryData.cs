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
using System.Collections.Generic;
using UnityEngine.Localization;

namespace LostSkiesDataDump.Data.Compendium;

public class CompendiumCategoryData(CompendiumCategory compendiumCategory)
{
    private readonly CompendiumCategory compendiumCategory = compendiumCategory;
    public string Id => compendiumCategory.Id;

    public string Name
    {
        get
        {
            Plugin.Log.LogDebug("CompendiumCategoryData.Name get {");
            Plugin.Log.LogDebug($"    CompendiumCategoryData: {this}");
            Plugin.Log.LogDebug($"    CompendiumCategoryData.compendiumCategory: {compendiumCategory}");
            Plugin.Log.LogDebug($"    CompendiumCategoryData.compendiumCategory.Id: {compendiumCategory.Id}");
            Plugin.Log.LogDebug($"    CompendiumCategoryData.compendiumCategory.Name: {compendiumCategory.Name}");
            Plugin.Log.LogDebug(compendiumCategory.Name);
            string localizedString = null;
            try
            {
                localizedString = compendiumCategory.Name?.GetLocalizedString();
                Plugin.Log.LogDebug($"    CompendiumCategoryData.compendiumCategory.Name?.GetLocalizedString(): {localizedString}");
                Plugin.Log.LogDebug(localizedString);
            }
            catch (Exception exception)
            {
                Plugin.Log.LogError($"Unable to get localized string for name of the {compendiumCategory.Id} compendium category");
                Plugin.Log.LogError(exception);
            }
            Plugin.Log.LogDebug("}");
            return localizedString;
        }
    }

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
