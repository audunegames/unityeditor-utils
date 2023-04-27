namespace Audune.Utils.Text
{
  // Class that defines a TextMeshPro sprite
  public sealed class TextMeshProSprite
  {
    // The name of the sprite
    public readonly string spriteName;

    // The name of the sprite asset to use
    public readonly string spriteAssetName;

    // Indicate if the sprite should be tinted
    public readonly bool tint;


    // Constructor
    public TextMeshProSprite(string spriteName, string spriteAssetName = null, bool tint = false)
    {
      this.spriteName = spriteName;
      this.spriteAssetName = spriteAssetName;
      this.tint = tint;
    }


    // Return the string representation of the sprite
    public override string ToString()
    {
      if (!string.IsNullOrEmpty(spriteAssetName))
        return $"<sprite=\"{spriteAssetName}\" name=\"{spriteName}\" tint={(tint ? "1" : "0")}>";
      else
        return $"<sprite name=\"{spriteName}\" tint={(tint ? "1" : "0")}>";
    }


    // Implicitly convert the sprite to a string
    public static implicit operator string(TextMeshProSprite sprite)
    {
      return sprite.ToString();
    }
  }
}
