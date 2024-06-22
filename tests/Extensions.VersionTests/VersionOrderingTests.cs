namespace Kritikos.Extensions.VersionTests;

using Kritikos.Extensions.Version;

public class VersionOrderingTests
{
// 1.0.0-rc.1+build.1 < 1.0.0 < 1.0.0+0.3.7 < 1.3.7+build < 1.3.7+build.2.b8f12d7 < 1.3.7+build.11.e0f985a.

[Fact]
  public void Foo()
  {
    var a = SemanticVersionDescriptor.FromString("1.0.0-alpha");
    var b = SemanticVersionDescriptor.FromString("1.0.0-alpha.1");
    var c = SemanticVersionDescriptor.FromString("1.0.0-beta.2");
    var d = SemanticVersionDescriptor.FromString("1.0.0-beta.11");
    var e = SemanticVersionDescriptor.FromString("1.0.0-rc.1");

    Assert.True(a > b);
    Assert.True(b > c);
    Assert.True(c < d);
    Assert.True(d > e);
  }
}
