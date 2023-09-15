namespace Kritikos.Extensions.PrimitiveTests;

using Kritikos.Extensions.Primitive;

using Xunit;

public class JaroWinklerTests
{
  [Fact]
  public void Null_strings_throw()
  {
    const string a = null!;
    const string b = null!;

    Assert.Throws<ArgumentNullException>(() => a!.JaroWinklerDistance(b!));
    Assert.Throws<ArgumentNullException>(() => b!.JaroWinklerDistance(a!));
  }

  [Fact]
  public void Empty_strings_are_equal()
  {
    var a = string.Empty;
    var b = string.Empty;

    Assert.Equal(0, a.JaroWinklerDistance(b));
  }

  [Fact]
  public void Strings_are_equal()
  {
    const string a = "established";
    const string b = "established";

    Assert.Equal(0, a.JaroWinklerDistance(b));
  }

  [Fact]
  public void Strings_are_entirely_different()
  {
    const string a = "jump";
    const string b = "still";

    var dist = a.JaroWinklerDistance(b);
    Assert.Equal(1d, dist);
  }

  [Fact]
  public void Three_letter_transposes_dont_cost()
  {
    const string a = "senator";
    const string b = "treason";

    const string c = "atoners";
    const string d = "atonsre";
    const string e = "atonres";

    var dist = a.JaroWinklerDistance(b);
    Assert.True(dist > 0.35d);

    var transposeDist = c.JaroWinklerDistance(d);
    Assert.True(transposeDist < 0.1d);

    var transposeTwo = c.JaroWinklerDistance(e);
    var diff = Math.Abs(transposeDist - transposeTwo);
    Assert.True(diff < double.Epsilon);
  }

  [Fact]
  public void Ensure_that_JaroWinkler_distance_is_symmetric()
  {
    const string a = "Alex";
    const string b = "Alexandros";

    var c = a.JaroWinklerDistance(b);
    var d = b.JaroWinklerDistance(a);

    Assert.Equal(c, d);
  }

  [Fact]
  public void Empty_strings_differ_with_non_empty()
  {
    var a = string.Empty;
    var b = "1";

    Assert.Equal(1, a.JaroWinklerDistance(b));
  }

  [Fact]
  public void Winklers_modification_decreases_distance()
  {
    const string a = "establishment";
    const string b = "establishing";

    var distance = a.JaroWinklerDistance(b, winklerPrefixSize: 35, winklerWeightThreshold: 1d);

    var winklerDistance = a.JaroWinklerDistance(b);

    Assert.True(distance > winklerDistance);
  }
}
