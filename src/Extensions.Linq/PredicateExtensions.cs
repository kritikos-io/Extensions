namespace Kritikos.Extensions.Linq;

using System.Linq.Expressions;

public static class PredicateExtensions
{
  public static Expression<Func<T, bool>> False<T>(this bool condition)
    => condition
      ? PredicateBuilder.False<T>()
      : PredicateBuilder.True<T>();
}
