namespace Kritikos.Extensions.Linq;

public class PagedResult<T>
{
  public List<T> Items { get; init; } = Array.Empty<T>().ToList();

  public int PageNumber { get; init; }

  public int PageSize { get; init; }

  public int TotalPages { get; init; }

  public int TotalCount { get; init; }

  public bool HasPreviousPage => PageNumber > 1;

  public bool HasNextPage => PageNumber < TotalPages;

  public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
}
