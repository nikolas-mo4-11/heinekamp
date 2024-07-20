using Heinekamp.Domain.Models;

namespace Heinekamp.PgDb.Repository.Interfaces;

public interface IFileTypeRepository
{
    Task<FileType> GetByExtension(string extension);
}