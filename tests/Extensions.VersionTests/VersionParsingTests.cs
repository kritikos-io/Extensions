namespace Kritikos.Extensions.VersionTests;

using System.Reflection;

using Kritikos.Extensions.Version;

public class VersionParsingTests
{
  [Fact]
  public void Assembly_version()
  {
    var assembly = typeof(VersionParsingTests).Assembly;
    var attrib = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!;
    var sutAttribute = SemanticVersionDescriptor.FromInformationalVersionAttribute(attrib);
    var sutAssembly = SemanticVersionDescriptor.FromAssembly(assembly);

    Assert.Equal(attrib.InformationalVersion, sutAttribute.ToString());
    Assert.Equal(attrib.InformationalVersion, sutAssembly.ToString());
  }

  [Fact]
  public void Stable_version_without_metadata()
  {
    var version = "12.3.85";
    var sut = SemanticVersionDescriptor.FromString(version);
    Assert.Equal(12, sut.Major);
    Assert.Equal(3, sut.Minor);
    Assert.Equal(85, sut.Patch);
    Assert.Empty(sut.Branch);
    Assert.Empty(sut.Sha1);
    Assert.Equal(version, sut.ToString());
  }

  [Fact]
  public void Stable_version_with_sha()
  {
    var version = "12.3.85+9eddbc9ace8eb8ef8795b69ba8afb02c3094b270";
    var sut = SemanticVersionDescriptor.FromString(version);
    Assert.Equal(12, sut.Major);
    Assert.Equal(3, sut.Minor);
    Assert.Equal(85, sut.Patch);
    Assert.Empty(sut.Branch);
    Assert.Equal("9eddbc9ace8eb8ef8795b69ba8afb02c3094b270", sut.Sha1);
    Assert.Equal(version, sut.ToString());
  }

  [Fact]
  public void Stable_version_with_full_metadata()
  {
    var version = "0.1.1+Branch.master.Sha.9eddbc9ace8eb8ef8795b69ba8afb02c3094b270";
    var sut = SemanticVersionDescriptor.FromString(version);
    Assert.Equal(0, sut.Major);
    Assert.Equal(1, sut.Minor);
    Assert.Equal(1, sut.Patch);
    Assert.Equal("master", sut.Branch);
    Assert.Equal("9eddbc9ace8eb8ef8795b69ba8afb02c3094b270", sut.Sha1);
    Assert.Equal(version, sut.ToString());
  }

  [Fact]
  public void Prerelease_version_without_metadata()
  {
    var version = "1.2.3-413213";
    var sut = SemanticVersionDescriptor.FromString(version);
    Assert.Equal(1, sut.Major);
    Assert.Equal(2, sut.Minor);
    Assert.Equal(3, sut.Patch);
    Assert.Empty(sut.Branch);
    Assert.Empty(sut.Sha1);
    Assert.Equal(version, sut.ToString());
  }

  [Fact]
  public void Prerelease_version_with_full_metadata()
  {
    var version = "13.2.1-328968.FixupAccount.35+Branch.feature/test.Sha.9eddbc9ace8eb8ef8795b69ba8afb02c3094b270";
    var sut = SemanticVersionDescriptor.FromString(version);
    Assert.Equal(13, sut.Major);
    Assert.Equal(2, sut.Minor);
    Assert.Equal(1, sut.Patch);
    Assert.Equal("328968.FixupAccount.35", sut.PreReleaseTag);
    Assert.Equal("328968", sut.WorkItem);
    Assert.Equal(35, sut.CommitCounter);
    Assert.Equal("feature/test", sut.Branch);
    Assert.Equal("9eddbc9ace8eb8ef8795b69ba8afb02c3094b270", sut.Sha1);
    Assert.Equal(version, sut.ToString());
  }

  [Fact]
  public void Prerelease_version_with_sha()
  {
    var version = "12.74.2-PullRequest226923.10+65650bba4fecd7bcab06347432d481a0f7c3c624";
    var sut = SemanticVersionDescriptor.FromString(version);
    Assert.Equal(12, sut.Major);
    Assert.Equal(74, sut.Minor);
    Assert.Equal(2, sut.Patch);
    Assert.Equal("PullRequest226923.10", sut.PreReleaseTag);
    Assert.Empty(sut.WorkItem);
    Assert.Equal(10, sut.CommitCounter);
    Assert.Equal(string.Empty, sut.Branch);
    Assert.Equal("65650bba4fecd7bcab06347432d481a0f7c3c624", sut.Sha1);
    Assert.Equal(version, sut.ToString());
  }
}
