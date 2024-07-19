namespace Heinekamp.MsDb.Context.Interfaces;

public interface IRepositoryContextFactory
{
    RepositoryContext CreateDbContext();
}