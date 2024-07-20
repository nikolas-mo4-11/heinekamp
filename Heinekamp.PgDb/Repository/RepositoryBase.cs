using Heinekamp.PgDb.Context;
using Microsoft.EntityFrameworkCore.Design;

namespace Heinekamp.PgDb.Repository;

public abstract class RepositoryBase
{
    protected IDesignTimeDbContextFactory<PgContext> ContextFactory { get; }

    protected RepositoryBase(IDesignTimeDbContextFactory<PgContext> contextFactory)
    {
        ContextFactory = contextFactory;
    }
}