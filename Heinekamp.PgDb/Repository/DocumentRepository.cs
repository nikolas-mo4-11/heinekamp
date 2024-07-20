using Heinekamp.Domain.Models;
using Heinekamp.PgDb.Context;
using Heinekamp.PgDb.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Heinekamp.PgDb.Repository;

public class DocumentRepository(IDesignTimeDbContextFactory<PgContext> contextFactory)
    : RepositoryBase(contextFactory), IDocumentRepository
{
    public async Task<Page<Document>> GetPageOfDocuments(int currentPage, int pageSize)
    {
        var result = new Page<Document> { CurrentPage = currentPage, PageSize = pageSize };

        await using var context = ContextFactory.CreateDbContext(null);
        var query = context.Documents.AsQueryable();
        
        result.TotalPagesCount = await query.CountAsync();
            
        query = query
            .Include(d => d.FileType)
            .OrderByDescending(d => d.CreatedDate)
            .Skip(currentPage * pageSize)
            .Take(pageSize);
            
        result.Records = await query.ToListAsync();

        return result;
    }

    public async Task Create(string name, FileType type)
    {
        await using var context = ContextFactory.CreateDbContext(null);
        context.Documents.Add(new Document
        {
            Name = name,
            CreatedDate = DateTime.UtcNow,
            FileType = type,
            DownloadsCount = 0
        });
        
        await context.SaveChangesAsync();
    }
}