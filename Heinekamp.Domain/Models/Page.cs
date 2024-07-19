namespace Heinekamp.Domain.Models;

public class Page<T>
{
    public long CurrentPage { get; set; }
    public long PageSize { get; set; }
    public List<T> Records { get; set; }
    public long TotalPagesCount { get; set; }
}