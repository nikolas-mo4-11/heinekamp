namespace Heinekamp.Domain.Models;

public class Document
{
    public long Id { get; set; }
    public string Name { get; set; }
    public FileType FileType { get; set; }
    public DateTime CreatedDate { get; set; }
    public long DownloadsCount { get; set; }
}