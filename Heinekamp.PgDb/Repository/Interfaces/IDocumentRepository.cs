using Heinekamp.Domain.Models;
using Heinekamp.Dtos;

namespace Heinekamp.PgDb.Repository.Interfaces;

public interface IDocumentRepository
{
    Task<List<Document>> ListAllDocumentsAsync();
    Task<Document> CreateAsync(string name, FileType type);
    Task UpdateDocumentAsync(UpdateDocumentRequestDto request);
    Task DeleteAsync(long id);
    Task<Document> GetByIdAsync(long id);
}