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
using UnityEngine.SceneManagement;

namespace LostSkiesDataDump.Converters.UnityEngine.SceneManagement;

[RequiresPreviewFeatures]
public class SceneConverter : BaseConverter<Scene>, IConverterDefault<SceneConverter>
{
    public static SceneConverter Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        Scene value,
        JsonSerializerOptions options
    )
    {
        WriteProperty(writer, value.handle, options);
        WriteProperty(writer, value.guid, options);
        WriteProperty(writer, value.path, options);
        WriteProperty(writer, value.name, options);
        WriteProperty(writer, value.isLoaded, options);
        WriteProperty(writer, value.buildIndex, options);
        WriteProperty(writer, value.rootCount, options);
        WriteProperty(writer, value.loadingState, options);
        WriteProperty(writer, value.isDirty, options);
        WriteProperty(writer, value.dirtyID, options);
        WriteProperty(writer, value.isSubScene, options);
        WriteProperty(writer, value.IsValid(), options);
        if (value.IsValid())
            WriteArray(writer, value.GetRootGameObjects(), options);
    }
}
