using System.Collections.Generic;
using UnityEngine;

namespace Audune.Utils.Sprites
{
  // Class that defines a sprite sheet
  public class SpriteSheet : ScriptableObject
  {
    // Sprite sheet settings
    [Tooltip("List of sprites in the sprite sheet")]
    public List<SpriteSheetEntry> sprites = new List<SpriteSheetEntry>();
  }
}