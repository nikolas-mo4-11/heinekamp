using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Heinekamp.PgDb.Context;

public class PgContextFactory : IDesignTimeDbContextFactory<PgContext>
{
    public PgContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<PgContext>();
        builder.UseNpgsql(configuration.GetConnectionString("PgDbConnectionString"));

        return new PgContext(builder.Options);
    }
}