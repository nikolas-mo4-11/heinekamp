using Heinekamp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Heinekamp.MsDb.Context;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
    {

    }

    public DbSet<Document> Documents { get; set; }
    public DbSet<FileType> FileTypes { get; set; }
    public DbSet<DownloadLink> DownloadLinks { get; set; }
}