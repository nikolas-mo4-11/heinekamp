using Heinekamp.Domain.Models;
using Heinekamp.PgDb.Repository.Interfaces;
using Heinekamp.Services.Interfaces;

namespace Heinekamp.Services;

public class DocumentService(IDocumentRepository documentRepository, IFileTypeRepository fileTypeRepository) : IDocumentService
{
    public Task<Page<Document>> GetPageOfDocuments(int pageIndex)
    {
        return documentRepository.GetPageOfDocuments(pageIndex, 5);
    }

    public async Task Create(string name)
    {
        var dotIndex = name.LastIndexOf('.');
        var fileType = await fileTypeRepository.GetByExtension(name[dotIndex..]);
        
        await documentRepository.Create(name[..dotIndex], fileType);
    }
}