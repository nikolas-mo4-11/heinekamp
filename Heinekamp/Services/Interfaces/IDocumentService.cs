using Heinekamp.Domain.Models;

namespace Heinekamp.Services.Interfaces;

public interface IDocumentService
{
    Task<Page<Document>> GetPageOfDocuments(int pageIndex);
    Task Create(string name);
}