using Heinekamp.Domain.Models;
using Heinekamp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Heinekamp.Controllers;

[Route("api/[controller]")]
public class DocumentApiController(IDocumentService documentService) : Controller
{
    [Route("page")]
    [HttpGet]
    public async Task<Page<Document>> GetPageOfDocuments(int pageIndex)
    {
        if (pageIndex < 0) throw new ArgumentException("Negative page number");
        
        return await documentService.GetPageOfDocuments(pageIndex);
    }
}