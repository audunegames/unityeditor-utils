using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Audune.Utils.Sprites
{
  // Class that defines an importer for sliced sprite data
  [ScriptedImporter(202103001, new string[] { "spritesheet" })]
  public class SpriteSheetImporter : ScriptedImporter
  {
    // Import the asset
    public override void OnImportAsset(AssetImportContext ctx)
    {
      var data = ScriptableObject.CreateInstance<SpriteSheet>();
      LoadSpriteData(data, ctx.assetPath);

      ctx.AddObjectToAsset(data.name, data);
      ctx.SetMainObject(data);
    }


    // Load a sprite sheet from a file
    public static void LoadSpriteData(SpriteSheet spriteSheet, string path)
    {
      spriteSheet.sprites = CSVFile.Read(path, DeserializeSpriteData).ToList();
    }

    // Save a sprite sheet to a file
    public static void SaveSpriteData(SpriteSheet spriteSheet, string path)
    {
      CSVFile.Write(path, spriteSheet.sprites, SerializeSpriteData);
    }


    // Deserialize a CSV entry to sprite data
    private static SpriteData DeserializeSpriteData(Dictionary<string, object> entry)
    {
      if (!entry.TryGetValueAsString("name", out var name))
        throw new ArgumentException("The record does not contain a \"name\" value");

      if (!entry.TryGetValueAsFloat("rect_x", out var rectX))
        throw new ArgumentException("The record does not contain a \"rect_x\" value");
      if (!entry.TryGetValueAsFloat("rect_y", out var rectY))
        throw new ArgumentException("The record does not contain a \"rect_y\" value");
      if (!entry.TryGetValueAsFloat("rect_width", out var rectWidth))
        throw new ArgumentException("The record does not contain a \"rect_width\" value");
      if (!entry.TryGetValueAsFloat("rect_height", out var rectHeight))
        throw new ArgumentException("The record does not contain a \"rect_height\" value");
      var rect = new Rect(rectX, rectY, rectWidth, rectHeight);

      Vector2? pivot = null;
      if (entry.TryGetValueAsFloat("pivot_x", out var pivotX)
        && entry.TryGetValueAsFloat("pivot_y", out var pivotY))
        pivot = new Vector2(pivotX, pivotY);

      Vector4? border = null;
      if (entry.TryGetValueAsFloat("border_left", out var borderLeft)
        && entry.TryGetValueAsFloat("border_right", out var borderRight)
        && entry.TryGetValueAsFloat("border_top", out var borderTop)
        && entry.TryGetValueAsFloat("border_bottom", out var borderBottom))
        border = new Vector4(borderLeft, borderBottom, borderRight, borderTop);

      return new SpriteData(name, rect, pivot, border);
    }

    // Serialize sprite data to a CSV entry
    private static Dictionary<string, object> SerializeSpriteData(SpriteData sprite)
    {
      var entry = new Dictionary<string, object>();

      entry["name"] = sprite.name;

      entry["rect_x"] = sprite.rect.x;
      entry["rect_y"] = sprite.rect.y;
      entry["rect_width"] = sprite.rect.width;
      entry["rect_height"] = sprite.rect.height;

      if (sprite.pivot != Vector2.zero)
      {
        entry["pivot_x"] = sprite.pivot.x;
        entry["pivot_y"] = sprite.pivot.y;
      }

      if (sprite.border != Vector4.zero)
      {
        entry["border_left"] = sprite.border.x;
        entry["border_right"] = sprite.border.z;
        entry["border_top"] = sprite.border.w;
        entry["border_bottom"] = sprite.border.y;
      }

      return entry;
    }
  }
}