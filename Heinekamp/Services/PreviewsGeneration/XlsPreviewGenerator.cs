using Heinekamp.Services.Interfaces;
using Spire.Xls; 

namespace Heinekamp.Services.PreviewsGeneration;

public class XlsPreviewGenerator : IPreviewGenerator
{
    public void CreatePreview(string inputDir, string outputDir)
    {
        var tempFileDir = $"temps\\temp_{Guid.NewGuid()}.pdf";

        using (var workbook = new Workbook())
        {
            workbook.LoadFromFile(inputDir); 
            workbook.ConverterSetting.SheetFitToPage = true; 
            workbook.SaveToFile(tempFileDir, FileFormat.PDF); 
        } 
        
        PreviewGeneratorResolver.GetGenerator(".pdf").CreatePreview(tempFileDir, outputDir);
        if (File.Exists(tempFileDir))
            File.Delete(tempFileDir);
    }
}