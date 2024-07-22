using Heinekamp.Services.Interfaces;

namespace Heinekamp.Services.PreviewsGeneration;

public static class PreviewGeneratorResolver
{
    private static readonly DocPreviewGenerator DocPreviewGenerator = new();
    private static readonly PdfPreviewGenerator PdfPreviewGenerator = new();
    private static readonly XlsPreviewGenerator XlsPreviewGenerator = new();
    private static readonly TxtPreviewGenerator TxtPreviewGenerator = new();
    private static readonly ImagePreviewGenerator ImagePreviewGenerator = new();
    
    public static IPreviewGenerator GetGenerator(string extension)
    {
        return extension switch
        {
            ".doc" or ".docx" => DocPreviewGenerator,
            ".pdf" => PdfPreviewGenerator,
            ".xls" or ".xlsx" => XlsPreviewGenerator,
            ".txt" => TxtPreviewGenerator,
            ".jpg" or ".jpeg" or ".png" or ".gif" => ImagePreviewGenerator,
            
            _ => throw new ArgumentOutOfRangeException(nameof(extension), "Format isn't supported")
        };
    }
}