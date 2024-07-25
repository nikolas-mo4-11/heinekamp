namespace Heinekamp.Dtos;

public class FileDownloadInfoDto
{
    public string FileName { get; set; }
    public byte[] Bytes { get; set; }
    public string MimeType { get; set; }
}