using System.Collections.Concurrent;
using System.IO.Compression;
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
        var filePath = GetFilePathByDocumentIdAndExt(documentEntity.Id, fileType.Extension);
            
        await using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);
        
        PreviewGeneratorResolver.GetGenerator(fileType.Extension)
            .CreatePreview(filePath, GetPreviewPathByDocumentId(documentEntity.Id));

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
        var filePath = GetFilePathByDocumentIdAndExt(id, extension);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"file {filePath} not found"); 
        
        File.Delete(filePath);
    }

    public async Task<DownloadLink> CreateLinkAsync(long docId, DateTime expires)
    {
        return await downloadLinkRepository.CreateAsync(docId, expires, GetLinkByGuid(Guid.NewGuid().ToString()));
    }

    public async Task<FileDownloadInfoDto?> GetFileDownloadInfoByLinkGuidAsync(string guid)
    {
        var link = await downloadLinkRepository.GetByLinkAsync(GetLinkByGuid(guid));

        if (link.ExpirationDate.ToUniversalTime() < DateTime.UtcNow)
            return null;
        
        var extension = link.Document.FileType.Extension;
        var filePath = GetFilePathByDocumentIdAndExt(link.DocumentId, extension);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found");
        
        return new FileDownloadInfoDto
        {
            FileName = $"{link.Document.Name}{extension}",
            Bytes = await File.ReadAllBytesAsync(filePath),
            MimeType = GetMimeType(extension)
        };
    }

    public async Task<FileDownloadInfoDto?> GetFileDownloadInfoByIdAsync(long id)
    {
        var document = await documentRepository.GetByIdAsync(id);
        
        var extension = document.FileType.Extension;
        var filePath = GetFilePathByDocumentIdAndExt(id, extension);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found");

        await documentRepository.UpdateDocumentAsync(new UpdateDocumentRequestDto
        {
            Id = document.Id,
            Name = document.Name,
            DownloadsCount = document.DownloadsCount + 1
        });
        
        return new FileDownloadInfoDto
        {
            FileName = $"{document.Name}{extension}",
            Bytes = await File.ReadAllBytesAsync(filePath),
            MimeType = GetMimeType(extension)
        };
    }

    public async Task<FileDownloadInfoDto?> CreateDocsArchiveAndGetItsDownloadInfo(List<long> documentIds)
    {
        var zipFilePath = GetFilePathByDocumentIdAndExt(-1, ".zip");
        var fileDataList = new ConcurrentBag<(string fileName, byte[] data)>();

        var tasks = documentIds.Select(async id =>
        {
            var document = await documentRepository.GetByIdAsync(id);
            var filePath = GetFilePathByDocumentIdAndExt(id, document.FileType.Extension);
            
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found");

            await documentRepository.UpdateDocumentAsync(new UpdateDocumentRequestDto
            {
                Id = document.Id,
                Name = document.Name,
                DownloadsCount = document.DownloadsCount + 1
            });
            
            var data = await File.ReadAllBytesAsync(filePath);
            fileDataList.Add(($"{document.Name}{document.FileType.Extension}", data));
        });
        await Task.WhenAll(tasks);

        using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
        {
            foreach (var fileData in fileDataList)
            {
                var entry = zipArchive.CreateEntry(fileData.fileName);

                using (var entryStream = entry.Open())
                using (var memoryStream = new MemoryStream(fileData.data))
                {
                    await memoryStream.CopyToAsync(entryStream);
                }
            }
        }

        var memory = new MemoryStream();
        await using (var stream = new FileStream(zipFilePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;

        File.Delete(zipFilePath);

        return new FileDownloadInfoDto
        {
            Bytes = memory.ToArray(),
            MimeType = "application/zip",
            FileName = "files.zip"
        };
    }

    

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

    private string GetFilePathByDocumentIdAndExt(long id, string extension) =>
        Path.Combine(env.WebRootPath, appSettings.Value.DocumentStorageDir[2..], $"{id.ToString()}{extension}");
    
    private string GetPreviewPathByDocumentId(long id) =>
        Path.Combine(env.WebRootPath, appSettings.Value.PreviewStorageDir[2..], $"{id.ToString()}.png");

}


