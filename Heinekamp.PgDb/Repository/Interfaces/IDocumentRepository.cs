using Heinekamp.Domain.Models;

namespace Heinekamp.PgDb.Repository.Interfaces;

public interface IDocumentRepository
{
    Task<Page<Document>> GetPageOfDocuments(int currentPage, int pageSize);
    Task Create(string name, FileType type);
}