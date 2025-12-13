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
using UnityEngine;

namespace LostSkiesDataDump.Converters.UnityEngine;

[RequiresPreviewFeatures]
public class SpriteConverter : ObjectConverter<Sprite>, IConverterDefault<SpriteConverter>
{
    public static new SpriteConverter Default { get; } = new();

    public override void WriteObjectBody(
        Utf8JsonWriter writer,
        Sprite value,
        JsonSerializerOptions options
    )
    {
        base.WriteObjectBody(writer, value, options);
        WriteProperty(writer, value.bounds, options);
        WriteProperty(writer, value.rect, options);
        WriteProperty(writer, value.border, options);
        // // public unsafe Texture2D texture
        // WriteProperty(writer, value.texture, options);
        WriteProperty(writer, value.extrude, options);
        WriteProperty(writer, value.pixelsPerUnit, options);
        WriteProperty(writer, value.spriteAtlasTextureScale, options);
        // // public unsafe Texture2D associatedAlphaSplitTexture
        // WriteProperty(writer, value.associatedAlphaSplitTexture, options);
        WriteProperty(writer, value.pivot, options);
        WriteProperty(writer, value.packed, options);
        WriteProperty(writer, value.packingMode, options);
        WriteProperty(writer, value.packingRotation, options);
        WriteProperty(writer, value.textureRect, options);
        WriteProperty(writer, value.textureRectOffset, options);
        WriteArray(writer, value.vertices, options);
        WriteArray(writer, value.triangles, options);
        WriteArray(writer, value.uv, options);
        WriteProperty(writer, value.GetPackingMode(), options);
        WriteProperty(writer, value.GetPackingRotation(), options);
        WriteProperty(writer, value.GetPacked(), options);
        WriteProperty(writer, value.GetTextureRect(), options);
        WriteProperty(writer, value.GetTextureRectOffset(), options);
        WriteProperty(writer, value.GetInnerUVs(), options);
        WriteProperty(writer, value.GetOuterUVs(), options);
        WriteProperty(writer, value.GetPadding(), options);
        // public unsafe Texture2D GetSecondaryTexture(int index)
        WriteProperty(writer, value.GetSecondaryTextureCount(), options);
        // public unsafe int GetSecondaryTextures(Il2CppReferenceArray<SecondarySpriteTexture> secondaryTexture)
        WriteProperty(writer, value.GetPhysicsShapeCount(), options);
        WriteProperty(writer, value.GetScriptableObjectsCount(), options);
        // public unsafe uint GetScriptableObjects(Il2CppReferenceArray<ScriptableObject> scriptableObjects)
        // public unsafe int GetPhysicsShapePointCount(int shapeIdx)
        // public unsafe int Internal_GetPhysicsShapePointCount(int shapeIdx)
        // public unsafe int GetPhysicsShape(int shapeIdx, List<Vector2> physicsShape)
        // public unsafe static void GetPhysicsShapeImpl(Sprite sprite, int shapeIdx, List<Vector2> physicsShape)
    }
}
