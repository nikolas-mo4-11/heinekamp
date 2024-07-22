using Heinekamp.Domain.Models;
using Heinekamp.Dtos;

namespace Heinekamp.Services.Interfaces;

public interface IDocumentService
{
    Task<Page<Document>> GetPageOfDocuments(int pageIndex);
    Task<IReadOnlyCollection<string>> CreateDocuments(IFormFileCollection files);
    Task<IReadOnlyCollection<FileType>> GetAvailableFileTypes();
    Task UpdateDocument(UpdateDocumentRequestDto request);
    Task DeleteDocument(long id);
}