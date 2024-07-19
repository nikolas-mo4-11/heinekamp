using Heinekamp.Domain.Models;

namespace Heinekamp.MsDb.Repository.Interfaces;

public interface IDocumentRepository
{
    Task<Page<Document>> GetPageOfDocuments(int currentPage, int pageSize);
}