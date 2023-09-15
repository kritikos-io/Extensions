namespace Kritikos.Extensions.VersionTests;

using System;

using Kritikos.Extensions.Version;

public class InvalidVersionTests
{
  [Fact]
  public void Null_version_throws()
    => Assert.Throws<ArgumentNullException>(() => SemanticVersionDescriptor.FromString(null!));

  [Fact]
  public void Invalid_major_number()
    => Assert.Throws<ArgumentException>(() => SemanticVersionDescriptor.FromString("a.0.0"));

  [Fact]
  public void Invalid_minor_number()
    => Assert.Throws<ArgumentException>(() => SemanticVersionDescriptor.FromString("1.a.0"));

  [Fact]
  public void Invalid_patch_number()
    => Assert.Throws<ArgumentException>(() => SemanticVersionDescriptor.FromString("5.2.a"));
}
