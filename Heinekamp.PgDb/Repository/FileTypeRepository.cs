using Heinekamp.Domain.Models;
using Heinekamp.PgDb.Context;
using Heinekamp.PgDb.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Heinekamp.PgDb.Repository;

public class FileTypeRepository(IDesignTimeDbContextFactory<PgContext> contextFactory)
    : RepositoryBase(contextFactory), IFileTypeRepository
{
    public async Task<FileType> GetByExtension(string extension)
    {
        await using var context = ContextFactory.CreateDbContext(null);

        return await context.FileTypes.AsQueryable().FirstOrDefaultAsync(t => t.Extension == extension) 
               ?? throw new ArgumentNullException(nameof(FileType), "Extension isn't being supported yet");
    }
    
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
}