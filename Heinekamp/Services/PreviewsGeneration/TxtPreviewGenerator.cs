using Heinekamp.Services.Interfaces;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

namespace Heinekamp.Services.PreviewsGeneration;

public class TxtPreviewGenerator : IPreviewGenerator
{
    public void CreatePreview(string inputDir, string outputDir)
    {
        var text = File.ReadAllText(inputDir);

        const int imageWidth = 1000;
        const int imageHeight = 1000;
        const int padding = 10;
        var fontPath = $"{Directory.GetCurrentDirectory()}\\calibri.ttf";

        using var image = new Image<Rgba32>(imageWidth, imageHeight);
        image.Mutate(ctx => ctx.Fill(Color.White));

        var fontCollection = new FontCollection();
        var fontFamily = fontCollection.Add(fontPath);
        var font = new Font(fontFamily, 24);

        var richTextOptions = new RichTextOptions(font)
        {
            Origin = new PointF(padding, padding),
            WrappingLength = imageWidth - 2 * padding,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };

        image.Mutate(ctx => ctx.DrawText(richTextOptions, text, Color.Black));
        image.Save(outputDir, new PngEncoder());
    }
}