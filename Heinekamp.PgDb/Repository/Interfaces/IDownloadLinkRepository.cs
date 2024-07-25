using Heinekamp.Domain.Models;

namespace Heinekamp.PgDb.Repository.Interfaces;

public interface IDownloadLinkRepository
{
    Task<DownloadLink> CreateAsync(long docId, DateTime expires, string link);
    Task<DownloadLink> GetByLinkAsync(string link);
}