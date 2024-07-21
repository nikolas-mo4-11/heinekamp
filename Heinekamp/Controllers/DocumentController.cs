using Heinekamp.Domain.AppConfig;
using Heinekamp.Domain.Models;
using Heinekamp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Heinekamp.Controllers;

[Route("api/[controller]")]
public class DocumentController(IDocumentService documentService, IWebHostEnvironment env, IOptions<AppSettings> appSettings) : Controller
{
    [Route("page")]
    [HttpGet]
    public async Task<Page<Document>> GetPageOfDocuments(int pageIndex)
    {
        if (pageIndex < 0) throw new ArgumentException("Negative page number");
        
        return await documentService.GetPageOfDocuments(pageIndex);
    }
    
    [Route("create")]
    [HttpPost]
    public async Task<ActionResult> CreateDocuments()
    {
        var files = Request.Form.Files;

        if (files.Count == 0)
        {
            return BadRequest("No files uploaded");
        }

        var uploads = Path.Combine(env.WebRootPath, appSettings.Value.DocumentStorageDir);
        if (!Directory.Exists(uploads))
        {
            Directory.CreateDirectory(uploads);
        }

        var fileNames = new List<string>();

        foreach (var file in files)
        {
            var filePath = Path.Combine(uploads, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            fileNames.Add(file.FileName);
        }

        return Ok(new { fileNames });
    }
}