using Heinekamp.Domain.Models;
using Heinekamp.Dtos;

namespace Heinekamp.PgDb.Repository.Interfaces;

public interface IDocumentRepository
{
    Task<List<Document>> ListAllDocuments();
    Task<Document> Create(string name, FileType type);
    Task UpdateDocument(UpdateDocumentRequestDto request);
    Task Delete(long id);
    Document GetById(long id);
}