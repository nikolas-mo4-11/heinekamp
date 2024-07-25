using Heinekamp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Heinekamp.PgDb.Context;

public class PgContext : DbContext
{
    public PgContext(DbContextOptions<PgContext> options) : base(options)
    {

    }
    
    public DbSet<Document> Documents { get; set; }
    public DbSet<FileType> FileTypes { get; set; }
    public DbSet<DownloadLink> DownloadLinks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Document>()
            .HasOne(d => d.FileType)
            .WithMany()
            .HasForeignKey(d => d.FileTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<DownloadLink>()
            .HasOne(dl => dl.Document)
            .WithMany()
            .HasForeignKey(dl => dl.DocumentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Document>().HasKey(d => d.Id);
        modelBuilder.Entity<Document>().Property(d => d.Id).ValueGeneratedOnAdd();
        
        modelBuilder.Entity<FileType>().HasKey(ft => ft.Id);
        
        modelBuilder.Entity<DownloadLink>().HasKey(dl => dl.Id);
        modelBuilder.Entity<DownloadLink>().Property(dl => dl.Id).ValueGeneratedOnAdd();
        
        SeedFileTypes(modelBuilder);
    }
    
    private void SeedFileTypes(ModelBuilder modelBuilder)
    {
        // need to be deleted before every migration
        var fileTypes = new List<FileType>
        {
            new() { Id = 1, Extension = "", IconFileName = "empty.svg" },
            new() { Id = 2, Extension = ".doc", IconFileName = "doc.svg" },
            new() { Id = 3, Extension = ".docx", IconFileName = "doc.svg" },
            new() { Id = 4, Extension = ".gif", IconFileName = "gif.svg" },
            new() { Id = 5, Extension = ".jpg", IconFileName = "jpg.svg" },
            new() { Id = 6, Extension = ".pdf", IconFileName = "pdf.svg" },
            new() { Id = 7, Extension = ".png", IconFileName = "png.svg" },
            new() { Id = 8, Extension = ".svg", IconFileName = "svg.svg" },
            new() { Id = 9, Extension = ".txt", IconFileName = "txt.svg" },
            new() { Id = 10, Extension = ".xls", IconFileName = "xls.svg" },
            new() { Id = 11, Extension = ".xlsx", IconFileName = "xls.svg" },
            new() { Id = 12, Extension = ".jpeg", IconFileName = "jpg.svg" }
        };

        modelBuilder.Entity<FileType>().HasData(fileTypes);
    }
}