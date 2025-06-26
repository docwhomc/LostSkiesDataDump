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
    public string EntryId => compendiumEntry.entryId;
    public string MainCategoryId => compendiumEntry.mainCategoryId;
    public string SubCategoryId => compendiumEntry.subCategoryId;
    public string MainCategoryRawString => compendiumEntry.mainCategoryRawString;
    public string EntryTitleRawString => compendiumEntry.entryTitleRawString;
    // TODO: mainCategoryString
    // TODO: subCategoryString
    // TODO: entryTitleString
    // TODO: bodyTextString
    public string ItemId => compendiumEntry.itemId;
    public string EntityId => compendiumEntry.entityId;
    public Il2CppStringArray UnlockIds => compendiumEntry.unlockIds;
    // TODO: icon
    public string VideoName => compendiumEntry.videoName;
    public string ConversationHistoryId => compendiumEntry.conversationHistoryId;
}
