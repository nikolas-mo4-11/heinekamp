using Heinekamp.Domain.Models;
using Heinekamp.Dtos;
using Heinekamp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Heinekamp.Controllers;

[Route("api/[controller]")]
public class DocumentController(IDocumentService documentService) : Controller
{
    [Route("all")]
    [HttpGet]
    public async Task<List<Document>> ListAllDocuments()// todo readonly
    {
        return await documentService.ListAllDocumentsAsync();
    }
    
    [Route("fileTypes")]
    [HttpGet]
    public async Task<IReadOnlyCollection<FileType>> GetFileTypes()
    {
        return await documentService.GetAvailableFileTypesAsync();
    }
    
    [Route("create")]
    [HttpPost]
    public async Task<ActionResult> CreateDocuments()
    {
        var files = Request.Form.Files;
        if (files.Count == 0)
            return BadRequest("No files uploaded");

        var uploadedFileNames = await documentService.CreateDocumentsAsync(files);
        return Ok(new {fileNames = uploadedFileNames });
    }
    
    [Route("update")]
    [HttpPost]
    public async Task<ActionResult> UpdateDocument([FromBody] UpdateDocumentRequestDto request)
    {
        if (request.Id <= 0)
            return BadRequest("Document id is zero or negative");

        await documentService.UpdateDocumentAsync(request);
        return Ok();
    }
    
    [Route("delete/{id}")]
    [HttpPost]
    public async Task<ActionResult> DeleteDocument(long id)
    {
        if (id <= 0)
            return BadRequest("Document id is zero or negative");

        await documentService.DeleteDocumentAsync(id);
        return Ok();
    }
    
    [Route("link/{docId}/expires/{expires}")]
    [HttpPost]
    public async Task<DownloadLink> CreateLink(long docId, DateTime expires)
    {
        if (docId <= 0)
            throw new ArgumentException("Document id is zero or negative");
        if (expires < DateTime.UtcNow)
            throw new ArgumentException("Expiration date is incorrect");

        return await documentService.CreateLinkAsync(docId, expires);
    }
    
    [HttpGet("dld/{guid}")]
    public async Task<IActionResult> DownloadFile(string guid)
    {
        var fileInfo = await documentService.GetFileDownloadInfoAsync(guid);

        if (fileInfo == null)
            return NotFound();
        
        return File(fileInfo.Bytes, fileInfo.MimeType, fileInfo.FileName); 
    }
    
}