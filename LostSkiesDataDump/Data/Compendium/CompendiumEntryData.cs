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

using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace LostSkiesDataDump.Data.Compendium;

public class CompendiumEntryData(CompendiumEntry compendiumEntry)
{
    private readonly CompendiumEntry compendiumEntry = compendiumEntry;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Consistency with wrapped class.")]
    public string entryId => compendiumEntry.entryId;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Consistency with wrapped class.")]
    public string mainCategoryId => compendiumEntry.mainCategoryId;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Consistency with wrapped class.")]
    public string subCategoryId => compendiumEntry.subCategoryId;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Consistency with wrapped class.")]
    public string mainCategoryRawString => compendiumEntry.mainCategoryRawString;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Consistency with wrapped class.")]
    public string entryTitleRawString => compendiumEntry.entryTitleRawString;
    // TODO: mainCategoryString
    // TODO: subCategoryString
    // TODO: entryTitleString
    // TODO: bodyTextString
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Consistency with wrapped class.")]
    public string itemId => compendiumEntry.itemId;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Consistency with wrapped class.")]
    public string entityId => compendiumEntry.entityId;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Consistency with wrapped class.")]
    public Il2CppStringArray unlockIds => compendiumEntry.unlockIds;
    // TODO: icon
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Consistency with wrapped class.")]
    public string videoName => compendiumEntry.videoName;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Consistency with wrapped class.")]
    public string conversationHistoryId => compendiumEntry.conversationHistoryId;
}
