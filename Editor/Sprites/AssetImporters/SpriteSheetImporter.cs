using Audune.Utils.Text;
using System.Linq;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Audune.Utils.Sprites.Editor
{
  // Class that defines an importer for sliced sprite data
  [ScriptedImporter(202303002, new string[] { "spritesheet" })]
  public class SpriteSheetImporter : ScriptedImporter
  {
    // Import the asset
    public override void OnImportAsset(AssetImportContext ctx)
    {
      // Create the sprite sheet
      var spriteSheet = ScriptableObject.CreateInstance<SpriteSheet>();

      // Read the sprite sheet
      var reader = new CSVReader<SpriteSheetEntry>(new SpriteSheetEntrySerializer());
      spriteSheet.sprites = reader.Read(ctx.assetPath).ToList();

      // Add the sprite sheet to the asset
      ctx.AddObjectToAsset(spriteSheet.name, spriteSheet);
      ctx.SetMainObject(spriteSheet);
    }
  }
}