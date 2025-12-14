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
using Il2CppInterop.Runtime.InteropTypes;

namespace LostSkiesDataDump.Converters.Il2CppInterop.Runtime.InteropTypes;

[RequiresPreviewFeatures]
public class Il2CppObjectBaseConverter<T>
    : BaseConverter<T>,
        IConverterDefault<Il2CppObjectBaseConverter<Il2CppObjectBase>>
    where T : Il2CppObjectBase
{
    public static Il2CppObjectBaseConverter<Il2CppObjectBase> Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options
    )
    {
        // internal bool isWrapped;
        // internal IntPtr pooledPtr;
        // private nint myGcHandle;
        WriteProperty(writer, value.ObjectClass, options);
        WriteProperty(writer, value.Pointer, options);
        WriteProperty(writer, value.WasCollected, options);
        // public T Cast<T>() where T : Il2CppObjectBase
        // internal static unsafe T UnboxUnsafe<T>(IntPtr pointer)
        // public T Unbox<T>() where T : unmanaged
        // private static readonly Type[] _intPtrTypeArray
        // private static readonly MethodInfo _getUninitializedObject
        // private static readonly MethodInfo _getTypeFromHandle
        // private static readonly MethodInfo _createGCHandle
        // private static readonly FieldInfo _isWrapped
        // public T? TryCast<T>() where T : Il2CppObjectBase
    }
}
