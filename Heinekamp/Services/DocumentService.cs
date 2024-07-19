using Heinekamp.Domain.Models;
using Heinekamp.MsDb.Repository.Interfaces;
using Heinekamp.Services.Interfaces;

namespace Heinekamp.Services.Classes;

public class DocumentService(IDocumentRepository documentRepository) : IDocumentService
{
    public Task<Page<Document>> GetPageOfDocuments(int pageIndex)
    {
        return documentRepository.GetPageOfDocuments(pageIndex, 5);
    }
}