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

    public async Task<IReadOnlyCollection<FileType>> GetAvailableFileTypes()
    {
        await using var context = ContextFactory.CreateDbContext(null);

        var temp = await context.FileTypes.AsQueryable().ToListAsync();
        return temp; //todo
    }
}