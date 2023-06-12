using System;
using Unity.Collections;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Audune.Utils.Sprites.Editor
{
  // Class that defines a raw texture
  public class RawTexture : IDisposable
  {
    // The color buffer
    public readonly NativeArray<Color32> colorBuffer;

    // The source texture information
    public readonly SourceTextureInformation information;


    // Return the size of the raw texture
    public int Width => information.width;
    public int Height => information.height;
    public Vector2Int Size => new Vector2Int(information.width, information.height);


    // Constructor
    public RawTexture(NativeArray<Color32> colorBuffer, SourceTextureInformation information)
    {
      this.colorBuffer = colorBuffer;
      this.information = information;
    }

    // Dispose of the color buffer information
    public void Dispose()
    {
      if (colorBuffer.IsCreated)
        colorBuffer.Dispose();
    }

    // Generate a texture
    public TextureGenerationOutput GenerateTexture(TextureGenerationSettings settings)
    {
      settings.sourceTextureInformation = information;
      return TextureGenerator.GenerateTexture(settings, colorBuffer);
    }


    // Load the color buffer from an image
    public static RawTexture Load(string path)
    {
      // Load the image
      var image = new System.Drawing.Bitmap(path, false);

      // Create the color buffer
      var colorBuffer = new NativeArray<Color32>(image.Width * image.Height, Allocator.Persistent);
      for (var y = 0; y < image.Height; y++)
      {
        for (var x = 0; x < image.Width; x++)
        {
          var color = image.GetPixel(x, y);
          colorBuffer[(image.Height - y - 1) * image.Width + x] = new Color32(color.R, color.G, color.B, color.A);
        }
      }

      // Return a source texture
      return new RawTexture(colorBuffer, new SourceTextureInformation { width = image.Width, height = image.Height, containsAlpha = true });
    }
  }
}