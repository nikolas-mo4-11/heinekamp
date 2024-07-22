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
    public Task<Page<Document>> GetPageOfDocuments(int pageIndex)
    {
        return documentRepository.GetPageOfDocuments(pageIndex, 5); //todo в конфиг
    }

    public async Task<IReadOnlyCollection<string>> CreateDocuments(IFormFileCollection files)
    {
        var uploadedFileNames = new List<string>();

        foreach (var file in files)
        {
            var fileName = file.FileName;

            // create in db
            var fileType = await fileTypeRepository.GetByExtension(Path.GetExtension(fileName));
            var documentEntity = await documentRepository.Create(Path.GetFileNameWithoutExtension(fileName), fileType);

            // upload to storage
            var filePath = GetFilePathByDocumentId(documentEntity.Id, fileType.Extension);
            
            await using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);
            
            // create preview image
            var temp = GetPreviewPathByDocumentId(documentEntity.Id); //todo
            var temp1 = PreviewGeneratorResolver.GetGenerator(fileType.Extension);
            temp1.CreatePreview(filePath, GetPreviewPathByDocumentId(documentEntity.Id));

            uploadedFileNames.Add(file.FileName);
        }

        return uploadedFileNames;
    }

    public async Task<IReadOnlyCollection<FileType>> GetAvailableFileTypes()
    {
        return await fileTypeRepository.GetAvailableFileTypes();
    }

    public Task UpdateDocument(UpdateDocumentRequestDto request)
    {
        return documentRepository.UpdateDocument(request);
    }

    public async Task DeleteDocument(long id)
    {
        // delete from db
        var extension = documentRepository.GetById(id).FileType.Extension;
        await documentRepository.Delete(id);

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


