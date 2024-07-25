using Heinekamp.Domain.Models;
using Heinekamp.PgDb.Context;
using Heinekamp.PgDb.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Heinekamp.PgDb.Repository;

public class DownloadLinkRepository(IDesignTimeDbContextFactory<PgContext> contextFactory)
    : RepositoryBase(contextFactory), IDownloadLinkRepository
{
    public async Task<DownloadLink> CreateAsync(long docId, DateTime expires, string link)
    {
        await using var context = ContextFactory.CreateDbContext(null);
        
        var newLink = new DownloadLink
        {
            DocumentId = docId,
            CreatedDate = DateTime.UtcNow,
            ExpirationDate = expires.ToUniversalTime(),
            Link = link
        };
        context.DownloadLinks.Add(newLink);
        
        await context.SaveChangesAsync();

        return newLink;
    }

    public async Task<DownloadLink> GetByLinkAsync(string link)
    {
        await using var context = ContextFactory.CreateDbContext(null);

        return await context.DownloadLinks
                   .AsQueryable()
                   .Include(l => l.Document)
                   .ThenInclude(d => d.FileType)
                   .FirstOrDefaultAsync(x => x.Link == link)
               ?? throw new ArgumentException($"Link  with link = {link} not found");
    }
}