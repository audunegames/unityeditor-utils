using UnityEngine;

namespace Audune.Utils.Unity.Editor
{ 
  // Class that defines utility methods for rects
  public static class RectUtils
  {
    // Expand a rect to the left
    public static Rect ExpandLeft(this Rect rect, float width)
    {
      return new Rect(rect.x - width, rect.y, rect.width + width, rect.height);
    }

    // Expand a rect to the right
    public static Rect ExpandRight(this Rect rect, float width)
    {
      return new Rect(rect.x, rect.y, rect.width + width, rect.height);
    }

    // Expand a rect to the the top
    public static Rect ExpandTop(this Rect rect, float height)
    {
      return new Rect(rect.x, rect.y - height, rect.width, rect.height + height);
    }

    // Expand a rect to the the bottom
    public static Rect ExpandBottom(this Rect rect, float height)
    {
      return new Rect(rect.x, rect.y, rect.width, rect.height + height);
    }


    // Contract a rect from the left
    public static Rect ContractLeft(this Rect rect, float width)
    {
      return new Rect(rect.x + width, rect.y, rect.width - width, rect.height);
    }

    // Contract a rect from the right
    public static Rect ContractRight(this Rect rect, float width)
    {
      return new Rect(rect.x, rect.y, rect.width - width, rect.height);
    }

    // Contract a rect from the top
    public static Rect ContractTop(this Rect rect, float height)
    {
      return new Rect(rect.x, rect.y + height, rect.width, rect.height - height);
    }

    // Contract a rect from the bottom
    public static Rect ContractBottom(this Rect rect, float height)
    {
      return new Rect(rect.x, rect.y, rect.width, rect.height - height);
    }


    // Align a rect to the left
    public static Rect AlignLeft(this Rect rect, float width)
    {
      return new Rect(rect.x, rect.y, width, rect.height);
    }

    // Align a rect to the right
    public static Rect AlignRight(this Rect rect, float width)
    {
      return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
    }

    // Align a rect to the top
    public static Rect AlignTop(this Rect rect, float height)
    {
      return new Rect(rect.x, rect.y, rect.width, height);
    }

    // Align a rect to the bottom
    public static Rect AlignBottom(this Rect rect, float height)
    {
      return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
    }


    // Align a rect to the left and store the remaining rect
    public static Rect AlignLeft(this Rect rect, float width, float separatorWidth, out Rect remainingRect)
    {
      remainingRect = rect.ContractLeft(width + separatorWidth);
      return rect.AlignLeft(width);
    }

    // Align a rect to the right and store the remaining rect
    public static Rect AlignRight(this Rect rect, float width, float separatorWidth, out Rect remainingRect)
    {
      remainingRect = rect.ContractRight(width + separatorWidth);
      return rect.AlignRight(width);
    }

    // Align a rect to the top and store the remaining rect
    public static Rect AlignTop(this Rect rect, float height, float separatorHeight, out Rect remainingRect)
    {
      remainingRect = rect.ContractTop(height + separatorHeight);
      return rect.AlignTop(height);
    }

    // Align a rect to the bottom and store the remaining rect
    public static Rect AlignBottom(this Rect rect, float height, float separatorHeight, out Rect remainingRect)
    {
      remainingRect = rect.ContractBottom(height + separatorHeight);
      return rect.AlignBottom(height);
    }
  }
}