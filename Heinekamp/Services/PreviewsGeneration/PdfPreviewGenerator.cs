using Heinekamp.Services.Interfaces;
using Spire.Pdf;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Heinekamp.Services.PreviewsGeneration;

public class PdfPreviewGenerator : IPreviewGenerator
{
    public void CreatePreview(string inputDir, string outputDir)
    {
        var pdf = new PdfDocument();
        pdf.LoadFromFile(inputDir);

        var memStream = new MemoryStream();
        pdf.SaveToImageStream(0, memStream, ".png");
        using var result = new Image<Rgba32>(1000, 1000);
        result.Mutate(x => x.DrawImage(Image.Load(memStream), new Point(0, 0), 1f));
        result.SaveAsPng(outputDir);
    }
}