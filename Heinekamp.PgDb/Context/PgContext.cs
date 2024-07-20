using Heinekamp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Heinekamp.PgDb.Context;

public class PgContext : DbContext
{
    public PgContext(DbContextOptions<PgContext> options) : base(options)
    {

    }
    
    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseNpgsql("Server=localhost;Database=heinekamp;User Id=postgres;Password=qwerty");

    }*/
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>().HasKey(d => d.Id);
        modelBuilder.Entity<Document>().Property(d => d.Id).ValueGeneratedOnAdd();
    }

    public DbSet<Document> Documents { get; set; }
    public DbSet<FileType> FileTypes { get; set; }
    public DbSet<DownloadLink> DownloadLinks { get; set; }
}