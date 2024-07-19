using Heinekamp.MsDb.Context.Interfaces;

namespace Heinekamp.MsDb.Repository;

public abstract class RepositoryBase
{
    protected IRepositoryContextFactory ContextFactory { get; }

    protected RepositoryBase(IRepositoryContextFactory contextFactory)
    {
        ContextFactory = contextFactory;
    }
}