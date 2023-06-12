using Audune.Utils.Text;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Utils.Sprites.Editor
{
  // Class that defines a record(de)serializer for sprite sheet entries
  public sealed class SpriteSheetEntrySerializer : IRecordDeserializer<SpriteSheetEntry>, IRecordSerializer<SpriteSheetEntry>
  {
    // Deserialize the sprite sheet entry
    public SpriteSheetEntry Deserialize(Dictionary<string, object> data)
    {
      if (!data.TryGetValueAsString("name", out var name))
        throw new ArgumentException("The record does not contain a \"name\" value");

      if (!data.TryGetValueAsFloat("rect_x", out var rectX))
        throw new ArgumentException("The record does not contain a \"rect_x\" value");
      if (!data.TryGetValueAsFloat("rect_y", out var rectY))
        throw new ArgumentException("The record does not contain a \"rect_y\" value");
      if (!data.TryGetValueAsFloat("rect_width", out var rectWidth))
        throw new ArgumentException("The record does not contain a \"rect_width\" value");
      if (!data.TryGetValueAsFloat("rect_height", out var rectHeight))
        throw new ArgumentException("The record does not contain a \"rect_height\" value");
      var rect = new Rect(rectX, rectY, rectWidth, rectHeight);

      Vector2? pivot = null;
      if (data.TryGetValueAsFloat("pivot_x", out var pivotX)
        && data.TryGetValueAsFloat("pivot_y", out var pivotY))
        pivot = new Vector2(pivotX, pivotY);

      Vector4? border = null;
      if (data.TryGetValueAsFloat("border_left", out var borderLeft)
        && data.TryGetValueAsFloat("border_right", out var borderRight)
        && data.TryGetValueAsFloat("border_top", out var borderTop)
        && data.TryGetValueAsFloat("border_bottom", out var borderBottom))
        border = new Vector4(borderLeft, borderBottom, borderRight, borderTop);

      return new SpriteSheetEntry(name, rect, pivot, border);
    }

    // Serialize the sprite sheet entry
    public Dictionary<string, object> Serialize(SpriteSheetEntry record)
    {
      var data = new Dictionary<string, object>();

      data["name"] = record.name;

      data["rect_x"] = record.rect.x;
      data["rect_y"] = record.rect.y;
      data["rect_width"] = record.rect.width;
      data["rect_height"] = record.rect.height;

      if (record.pivot != Vector2.zero)
      {
        data["pivot_x"] = record.pivot.x;
        data["pivot_y"] = record.pivot.y;
      }

      if (record.border != Vector4.zero)
      {
        data["border_left"] = record.border.x;
        data["border_right"] = record.border.z;
        data["border_top"] = record.border.w;
        data["border_bottom"] = record.border.y;
      }

      return data;
    }
  }
}