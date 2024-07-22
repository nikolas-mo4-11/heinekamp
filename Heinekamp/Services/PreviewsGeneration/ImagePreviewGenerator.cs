using Heinekamp.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace Heinekamp.Services.PreviewsGeneration;

public class ImagePreviewGenerator : IPreviewGenerator
{
    public void CreatePreview(string inputDir, string outputDir)
    {
        using var image = Image.Load<Rgba32>(inputDir);
        image.Save(outputDir, new PngEncoder());
    }
}