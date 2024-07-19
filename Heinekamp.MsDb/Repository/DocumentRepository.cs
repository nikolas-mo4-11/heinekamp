using Heinekamp.Domain.Models;
using Heinekamp.MsDb.Context.Interfaces;
using Heinekamp.MsDb.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Heinekamp.MsDb.Repository;

public class DocumentRepository(IRepositoryContextFactory contextFactory)
    : RepositoryBase(contextFactory), IDocumentRepository
{
    public async Task<Page<Document>> GetPageOfDocuments(int currentPage, int pageSize)
    {
        var result = new Page<Document> { CurrentPage = currentPage, PageSize = pageSize };

        await using var context = ContextFactory.CreateDbContext();
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