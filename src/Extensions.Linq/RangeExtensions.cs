namespace Kritikos.Extensions.Linq;

public static class RangeExtensions
{
  public static IntEnumerator GetEnumerator(this Range range) => new(range);
}
