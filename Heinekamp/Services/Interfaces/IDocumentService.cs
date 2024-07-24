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
}