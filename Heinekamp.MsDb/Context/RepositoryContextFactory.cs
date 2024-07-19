using Heinekamp.MsDb.Context.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Heinekamp.MsDb.Context;

public class RepositoryContextFactory(string connectionString) : IRepositoryContextFactory
{
    public RepositoryContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new RepositoryContext(optionsBuilder.Options);
    }
}