using Heinekamp.Services.Interfaces;
using Spire.Doc;

namespace Heinekamp.Services.PreviewsGeneration;

public class DocPreviewGenerator : IPreviewGenerator
{
    public void CreatePreview(string inputDir, string outputDir)
    {
        var tempFileDir = $"temps\\temp_{Guid.NewGuid()}.pdf";
        using (var document = new Document())
        {
            document.LoadFromFile(inputDir);
            document.SaveToFile(tempFileDir, FileFormat.PDF);
        }
        
        PreviewGeneratorResolver.GetGenerator(".pdf").CreatePreview(tempFileDir, outputDir);
        if (File.Exists(tempFileDir))
            File.Delete(tempFileDir);
    }
}