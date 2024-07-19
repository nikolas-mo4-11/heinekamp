namespace Heinekamp.Domain.Models;

public class DownloadLink
{
    public long Id { get; set; }
    public Document Document { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Link { get; set; }
}