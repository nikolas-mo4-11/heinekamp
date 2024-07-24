using Heinekamp.Domain.Models;
using Heinekamp.Dtos;
using Heinekamp.PgDb.Context;
using Heinekamp.PgDb.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Heinekamp.PgDb.Repository;

public class DocumentRepository(IDesignTimeDbContextFactory<PgContext> contextFactory)
    : RepositoryBase(contextFactory), IDocumentRepository
{
    public async Task<List<Document>> ListAllDocumentsAsync()
    {
        await using var context = ContextFactory.CreateDbContext(null);
        return await context.Documents.AsQueryable()
            .Include(d => d.FileType)
            .OrderByDescending(d => d.CreatedDate)
            .ToListAsync();
    }

    public async Task<Document> CreateAsync(string name, FileType type)
    {
        await using var context = ContextFactory.CreateDbContext(null);
        var newDocument = new Document
        {
            Name = name,
            CreatedDate = DateTime.UtcNow,
            FileTypeId = type.Id,
            DownloadsCount = 0
        };
        context.Documents.Add(newDocument);
        
        await context.SaveChangesAsync();

        return newDocument;
    }

    public async Task UpdateDocumentAsync(UpdateDocumentRequestDto request)
    {
        await using var context = ContextFactory.CreateDbContext(null);

        var documentToUpdate = context.Documents.FirstOrDefault(x => x.Id == request.Id);
        if (documentToUpdate == null)
            throw new ArgumentException($"Document with id = {request.Id} not found");

        documentToUpdate.Name = request.Name;
        documentToUpdate.DownloadsCount = request.DownloadsCount;

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        await using var context = ContextFactory.CreateDbContext(null);

        var documentToDelete = context.Documents.FirstOrDefault(x => x.Id == id);
        if (documentToDelete == null)
            throw new ArgumentException($"Document with id = {id} not found");
        
        context.Documents.Remove(documentToDelete);
        
        await context.SaveChangesAsync();
    }

    public async Task<Document> GetByIdAsync(long id)
    {
        await using var context = ContextFactory.CreateDbContext(null);

        return await context.Documents
                   .AsQueryable()
                   .Include(d => d.FileType)
                   .FirstOrDefaultAsync(x => x.Id == id)
               ?? throw new ArgumentException($"Document with id = {id} not found");
    }
}