using Heinekamp.Domain.AppConfig;
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
    IFileTypeRepository fileTypeRepository
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

    private string GetFilePathByDocumentId(long id, string extension) =>
        Path.Combine(env.WebRootPath, appSettings.Value.DocumentStorageDir[2..], $"{id.ToString()}{extension}");
    
    private string GetPreviewPathByDocumentId(long id) =>
        Path.Combine(env.WebRootPath, appSettings.Value.PreviewStorageDir[2..], $"{id.ToString()}.png");

}


