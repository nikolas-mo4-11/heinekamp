namespace Heinekamp.Dtos;

public class UpdateDocumentRequestDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long DownloadsCount { get; set; }
}