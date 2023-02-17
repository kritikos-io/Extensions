namespace Kritikos.Extensions.LinqTests;

using Kritikos.Extensions.Linq;
using Kritikos.Extensions.Linq.Samples;

using Xunit;

public class EnumerableIfTests
{
  private static IEnumerable<Animal> Animals { get; } = AnimalProvider.Animals
    .Generate(30)
    .AsEnumerable();

  [Fact]
  public void Null_exception_is_propagated()
  {
    IEnumerable<Animal> query = null!;

    Assert.Throws<ArgumentNullException>(() => query.WhereIf(true, x => x.Id != Guid.Empty).ToList());
    Assert.Throws<ArgumentNullException>(() => query.TakeIf(true, 3).ToList());
    Assert.Throws<ArgumentNullException>(() => query.SkipIf(true, 3).ToList());
  }

  [Fact]
  public void TakeIf()
  {
    var query = Animals;

    var result = query.TakeIf(true, 10).ToList();
    Assert.Equal(10, result.Count);
  }
}
