using Heinekamp.Domain.AppSettings;
using Heinekamp.Domain.Models;
using Heinekamp.Dtos;
using Heinekamp.PgDb.Repository.Interfaces;
using Heinekamp.Services.Interfaces;
using Heinekamp.Services.PreviewsGeneration;
using Microsoft.Extensions.Options;

namespace Heinekamp.Services;

public class DocumentService(
    IWebHostEnvironment env,
    IOptions<AppSettings> appSettings,
    IDocumentRepository documentRepository,
    IFileTypeRepository fileTypeRepository,
    IDownloadLinkRepository downloadLinkRepository
) : IDocumentService
{
    public async Task<List<Document>> ListAllDocumentsAsync()
    {
        return await documentRepository.ListAllDocumentsAsync();
    }

    public async Task<IReadOnlyCollection<string>> CreateDocumentsAsync(IFormFileCollection files)
    {
        var tasks = files.Select(CreateSingleDocumentAsync).ToList();
        return await Task.WhenAll(tasks);
    }

    private async Task<string> CreateSingleDocumentAsync(IFormFile file)
    {
        var fileName = file.FileName;

        // create in db
        var fileType = await fileTypeRepository.GetByExtension(Path.GetExtension(fileName));
        var documentEntity = await documentRepository.CreateAsync(Path.GetFileNameWithoutExtension(fileName), fileType);

        // upload to storage
        var filePath = GetFilePathByDocumentId(documentEntity.Id, fileType.Extension);
            
        await using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);
            
        // create preview image
        var temp = GetPreviewPathByDocumentId(documentEntity.Id); //todo
        var temp1 = PreviewGeneratorResolver.GetGenerator(fileType.Extension);
        temp1.CreatePreview(filePath, GetPreviewPathByDocumentId(documentEntity.Id));

        return file.FileName;
    }

    public async Task<IReadOnlyCollection<FileType>> GetAvailableFileTypesAsync()
    {
        return await fileTypeRepository.GetAvailableFileTypes();
    }

    public Task UpdateDocumentAsync(UpdateDocumentRequestDto request)
    {
        return documentRepository.UpdateDocumentAsync(request);
    }

    public async Task DeleteDocumentAsync(long id)
    {
        // delete from db
        var extension = (await documentRepository.GetByIdAsync(id)).FileType.Extension;
        await documentRepository.DeleteAsync(id);

        // delete from storage
        var filePath = GetFilePathByDocumentId(id, extension);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"file {filePath} not found"); 
        
        File.Delete(filePath);
    }

    public async Task<DownloadLink> CreateLinkAsync(long docId, DateTime expires)
    {
        return await downloadLinkRepository.CreateAsync(docId, expires, GetLinkByGuid(Guid.NewGuid().ToString()));
    }

    public async Task<FileDownloadInfoDto?> GetFileDownloadInfoAsync(string guid)
    {
        var link = await downloadLinkRepository.GetByLinkAsync(GetLinkByGuid(guid));

        if (link.ExpirationDate.ToUniversalTime() < DateTime.UtcNow)
            return null;
        
        var extension = link.Document.FileType.Extension;
        var filePath = GetFilePathByDocumentId(link.DocumentId, extension);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found");
        
        return new FileDownloadInfoDto
        {
            FileName = $"{link.Document.Name}{extension}",
            Bytes = await File.ReadAllBytesAsync(filePath),
            MimeType = GetMimeType(extension)
        };
    }//http://localhost:44320/api/document/dld/37870fdc-e752-4207-b0c5-c4cfa40ce326

    private string GetLinkByGuid(string guid) =>
        $"{appSettings.Value.Domain}/api/document/dld/{guid}";
    
    private static string GetMimeType(string fileExtension)
    {
        return fileExtension.ToLower() switch
        {
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".txt" => "text/plain",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            _ => "application/octet-stream"
        };
    }

    private string GetFilePathByDocumentId(long id, string extension) =>
        Path.Combine(env.WebRootPath, appSettings.Value.DocumentStorageDir[2..], $"{id.ToString()}{extension}");
    
    private string GetPreviewPathByDocumentId(long id) =>
        Path.Combine(env.WebRootPath, appSettings.Value.PreviewStorageDir[2..], $"{id.ToString()}.png");

}


