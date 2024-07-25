using Heinekamp.Domain.Models;
using Heinekamp.Dtos;

namespace Heinekamp.Services.Interfaces;

public interface IDocumentService
{
    Task<List<Document>> ListAllDocumentsAsync();
    Task<IReadOnlyCollection<string>> CreateDocumentsAsync(IFormFileCollection files);
    Task<IReadOnlyCollection<FileType>> GetAvailableFileTypesAsync();
    Task UpdateDocumentAsync(UpdateDocumentRequestDto request);
    Task DeleteDocumentAsync(long id);
    Task<DownloadLink> CreateLinkAsync(long docId, DateTime expires);
    Task<FileDownloadInfoDto?> GetFileDownloadInfoByLinkGuidAsync(string guid);
    Task<FileDownloadInfoDto?> GetFileDownloadInfoByIdAsync(long id);
    Task<FileDownloadInfoDto?> CreateDocsArchiveAndGetItsDownloadInfo(List<long> documentIds);
}