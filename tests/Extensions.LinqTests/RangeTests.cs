#pragma warning disable CA1707
namespace Kritikos.Extensions.LinqTests;

using Kritikos.Extensions.Linq;

using Xunit;

public class RangeTests
{
  [Fact]
  public void Range_enumeration_has_same_result_as_loop()
  {
    var extension = new List<int>();
    foreach (var number in 0..10)
    {
      extension.Add(number);
    }

    var classic = new List<int>();
    for (var j = 0; j <= 10; j++)
    {
      classic.Add(j);
    }

    Assert.Equal(classic, extension);
  }

  [Fact]
  public void Range_from_end_throws_not_supported() =>
    Assert.Throws<NotSupportedException>(() =>
    {
      foreach (var number in 10..)
      {
      }
    });
}
