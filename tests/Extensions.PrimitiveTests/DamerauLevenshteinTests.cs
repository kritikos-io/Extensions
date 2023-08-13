namespace Kritikos.Extensions.PrimitiveTests;

using Kritikos.Extensions.Primitive;

using Xunit;

public class DamerauLevenshteinTests
{
  [Fact]
  public void Null_strings_throw()
  {
    const string a = null!;
    const string b = null!;

    Assert.Throws<ArgumentNullException>(() => a!.DamerauLevenshteinDistance(b!));
    Assert.Throws<ArgumentNullException>(() => b!.DamerauLevenshteinDistance(a!));
  }

  [Fact]
  public void Empty_strings_are_equal()
  {
    var a = string.Empty;
    var b = string.Empty;

    Assert.Equal(0, a.DamerauLevenshteinDistance(b));
  }

  [Fact]
  public void Empty_string_is_equal_to_other_string_length()
  {
    const string a = "";
    const string b = "Alexandros";

    Assert.Equal(b.Length, a.DamerauLevenshteinDistance(b));

    Assert.Equal(b.Length, b.DamerauLevenshteinDistance(a));
  }

  [Fact]
  public void Insertions_from_empty()
  {
    var source = "";
    var target = "hello";
    Assert.Equal(5, source.DamerauLevenshteinDistance(target));
  }

  [Fact]
  void Single_transposition()
  {
    var source = "kitten";
    var target = "kittne";
    Assert.Equal(1, source.DamerauLevenshteinDistance(target));
  }
}
