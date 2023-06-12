using Audune.Utils.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.Sprites.Editor
{
  // Class that defines menu items for sprite sheets
  public static class SpriteSheetMenuItems
  {
    // Save the sprites in the selected asset to a sprite sheet
    [MenuItem("Assets/Audune Utilities/Save To Sprite Sheet Asset (.asset)")]
    public static void SaveSelectedToSpriteSheet()
    {
      var sprites = GetSpritesFromAsset(Selection.activeObject);
      if (sprites.Count() == 0)
        return;

      var spriteSheet = ConvertSpritesToSpriteSheet(sprites);

      var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
      var path = Path.GetFullPath(Path.Combine($"{Application.dataPath}/..", $"{assetPath.Substring(0, assetPath.LastIndexOf("."))}.asset"))
        .Replace('/', Path.DirectorySeparatorChar);

      path = EditorUtility.SaveFilePanelInProject($"Save sprite sheet asset for {Selection.activeObject.name}", path, "asset", $"Enter a file name to save the sprite sheet asset for {Selection.activeObject.name} to");
      if (!string.IsNullOrEmpty(path))
      {
        AssetDatabase.CreateAsset(spriteSheet, path);
        AssetDatabase.SaveAssets();
      }
    }

    [MenuItem("Assets/Audune Utilities/Save To Sprite Sheet Asset (.asset)", true)]
    public static bool ValidateSaveSelectedToSpriteSheet()
    {
      return GetSpritesFromAsset(Selection.activeObject).Count() > 0;
    }

    // Save the sprites in the selected asset to a sprite sheet file
    [MenuItem("Assets/Audune Utilities/Save To Sprite Sheet File (.spritesheet)")]
    public static void SaveSelectedToSpriteSheetFile()
    {
      var sprites = GetSpritesFromAsset(Selection.activeObject);
      if (sprites.Count() == 0)
        return;

      var spriteSheet = ConvertSpritesToSpriteSheet(sprites);

      var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
      var path = Path.GetFullPath(Path.Combine($"{Application.dataPath}/..", $"{assetPath.Substring(0, assetPath.LastIndexOf("."))}.spritesheet"))
        .Replace('/', Path.DirectorySeparatorChar);

      path = EditorUtility.SaveFilePanelInProject($"Save sprite sheet file for {Selection.activeObject.name}", path, "spritesheet", $"Enter a file name to save the sprite sheet file for {Selection.activeObject.name} to");
      if (!string.IsNullOrEmpty(path))
      {
        var writer = new CSVWriter<SpriteSheetEntry>(new SpriteSheetEntrySerializer());
        writer.Write(spriteSheet.sprites, path);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
      }
    }

    [MenuItem("Assets/Audune Utilities/Save To Sprite Sheet File (.spritesheet)", true)]
    public static bool ValidateSaveSelectedToSpriteSheetFile()
    {
      return GetSpritesFromAsset(Selection.activeObject).Count() > 0;
    }


    // Get the sprites from an asset
    private static IEnumerable<Sprite> GetSpritesFromAsset(Object asset)
    {
      var assetPath = AssetDatabase.GetAssetPath(asset);
      return assetPath != "" ? AssetDatabase.LoadAllAssetsAtPath(assetPath).OfType<Sprite>() : Enumerable.Empty<Sprite>();
    }

    // Convert a list of sprites to a sprite sheet
    private static SpriteSheet ConvertSpritesToSpriteSheet(IEnumerable<Sprite> sprites)
    {
      var spriteSheet = ScriptableObject.CreateInstance<SpriteSheet>();
      foreach (var sprite in sprites)
        spriteSheet.sprites.Add(new SpriteSheetEntry(sprite.name, sprite.rect, sprite.pivot, sprite.border));
      return spriteSheet;
    }
  }
}
