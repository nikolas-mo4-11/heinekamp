﻿using Heinekamp.Domain.Models;
using Heinekamp.Dtos;
using Heinekamp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Heinekamp.Controllers;

[Route("api/[controller]")]
public class DocumentController(IDocumentService documentService) : Controller
{
    [Route("all")]
    [HttpGet]
    public async Task<List<Document>> ListAllDocuments()
    {
        return await documentService.ListAllDocuments();
    }
    
    [Route("fileTypes")]
    [HttpGet]
    public async Task<IReadOnlyCollection<FileType>> GetFileTypes()
    {
        return await documentService.GetAvailableFileTypes();
    }
    
    [Route("create")]
    [HttpPost]
    public async Task<ActionResult> CreateDocuments()
    {
        var files = Request.Form.Files;
        if (files.Count == 0)
            return BadRequest("No files uploaded");

        var uploadedFileNames =   await documentService.CreateDocuments(files);
        return Ok(new {fileNames = uploadedFileNames });
    }
    
    [Route("update")]
    [HttpPost]
    public async Task<ActionResult> UpdateDocument([FromBody] UpdateDocumentRequestDto request)
    {
        if (request.Id <= 0)
            return BadRequest("Document id is zero or negative");

        await documentService.UpdateDocument(request);
        return Ok();
    }
    
    [Route("delete/{id}")]
    [HttpPost]
    public async Task<ActionResult> DeleteDocument(long id)
    {
        if (id <= 0)
            return BadRequest("Document id is zero or negative");

        await documentService.DeleteDocument(id);
        return Ok();
    }
}