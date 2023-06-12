using Audune.Utils.Sprites;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Audune.Utils.Sprites.Editor
{
  // Class that defines an importer for a texture containing a sprite sheet
  [ScriptedImporter(202203002, new string[] { }, new string[] { "bmp", "gif", "jpg", "jpeg", "png", "tif", "tiff" })]
  public class SpriteSheetTextureImporter : ScriptedImporter
  {
    // Importer settings
    [Tooltip("The sprite sheet to slice the texture into")]
    public SpriteSheet spriteSheet;

    [Header("Sprite settings")]
    [Tooltip("The number of pixels in the sprite that correspond to one unit in world space")]
    public float pixelsPerUnit = 100;
    [Tooltip("The type of sprite mesh to generate")]
    public SpriteMeshType meshType = SpriteMeshType.Tight;
    [Tooltip("The number of blank pixels to leave between the edge of the sprite and the mesh"), Range(0, 32)]
    public uint extrudeEdges = 1;
    [Tooltip("Toggle to generate a default physics shape from the outline of the sprite")]
    public bool generatePhysicsShape = true;

    [Header("Texture settings")]
    [Tooltip("The wrapping mode of texture coordinates")]
    public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
    [Tooltip("The filter mode of the texture")]
    public FilterMode filterMode = FilterMode.Bilinear;

    [Header("Advanced texture settings")]
    [Tooltip("Toggle if the texture should store data in sRGB (gamma) color space")]
    public bool sRGBTexture = true;
    [Tooltip("The manner how the alpha of the imported texture is generated")]
    public TextureImporterAlphaSource alphaSource = TextureImporterAlphaSource.FromInput;
    [Tooltip("Toggle to handle the aplha channel as transparency in the texture")]
    public bool alphaIsTransparency = true;
    [Tooltip("Toggle to ignore the gamma in PNG files")]
    public bool ignorePNGGamma = false;
    [Tooltip("Toggle to make the raw texture data readable from scripts")]
    public bool readableFromScripts = false;
    [Tooltip("Toggle to generate mip maps for the texture")]
    public bool generateMipMaps = false;


    // Import the asset
    public override void OnImportAsset(AssetImportContext ctx)
    {
      // Generate the source texture
      var rawTexture = RawTexture.Load(ctx.assetPath);

      // Create the texture settings
      var textureSettings = new TextureGenerationSettings(TextureImporterType.Sprite);

      textureSettings.textureImporterSettings.spritePixelsPerUnit = pixelsPerUnit;
      textureSettings.textureImporterSettings.spriteMeshType = meshType;
      textureSettings.textureImporterSettings.spriteExtrude = extrudeEdges;
      textureSettings.textureImporterSettings.spriteGenerateFallbackPhysicsShape = generatePhysicsShape;
      textureSettings.textureImporterSettings.wrapMode = wrapMode;
      textureSettings.textureImporterSettings.filterMode = filterMode;
      textureSettings.textureImporterSettings.aniso = 1;
      textureSettings.textureImporterSettings.sRGBTexture = sRGBTexture;
      textureSettings.textureImporterSettings.alphaSource = alphaSource;
      textureSettings.textureImporterSettings.alphaIsTransparency = alphaIsTransparency;
      textureSettings.textureImporterSettings.ignorePngGamma = ignorePNGGamma;
      textureSettings.textureImporterSettings.readable = readableFromScripts;
      textureSettings.textureImporterSettings.mipmapEnabled = generateMipMaps;

      if (spriteSheet != null)
      {
        textureSettings.spriteImportData = spriteSheet.sprites.Select((sprite, index) => new SpriteImportData {
          name = sprite.name,
          rect = sprite.rect,
          pivot = sprite.pivot,
          border = sprite.border
        }).ToArray();
      }

      // Create the texture
      var textureOutput = rawTexture.GenerateTexture(textureSettings);

      foreach (var importWarning in textureOutput.importWarnings)
        Debug.LogWarning(importWarning, ctx.mainObject);
      foreach (var importInspectorWarning in textureOutput.importInspectorWarnings)
        Debug.LogWarning(importInspectorWarning, ctx.mainObject);

      if (textureOutput.output != null)
      {
        ctx.AddObjectToAsset("texture", textureOutput.output, textureOutput.thumbNail);
        ctx.SetMainObject(textureOutput.output);

        foreach (var sprite in textureOutput.sprites)
          ctx.AddObjectToAsset(sprite.name, sprite);
      }

      // Dispose of the source texture
      rawTexture.Dispose();
    }
  }
}
