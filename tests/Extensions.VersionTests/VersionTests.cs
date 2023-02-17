namespace Kritikos.Extensions.VersionTests;

using Kritikos.Extensions.Version;

public class VersionTests
{
  [Theory]
  [InlineData("1.0.0+Branch.master.Sha.65650bba4fecd7bcab06347432d481a0f7c3c624")]
  [InlineData("1.2.30+d762e7afc64cffdb53fc2d656bcfa7200faaa8bc")]
  public void Ensure_stable_version_parses(string version)
  {
    var sut = SemanticVersionDescriptor.FromString(version);
    Assert.NotNull(sut);
    Assert.Equal(version, sut.ToString());
  }

  [Theory]
  [InlineData(
    "1.10.0-363996-tax-deduction-details.45+Branch.feature-363996-tax-deduction-details.Sha.9eddbc9ace8eb8ef8795b69ba8afb02c3094b270")]
  [InlineData("1.10.0-363996-tax-deduction-details.45+9eddbc9ace8eb8ef8795b69ba8afb02c3094b270")]
  public void Ensure_feature_branch_parses(string version)
  {
    var sut = SemanticVersionDescriptor.FromString(version);
    Assert.NotNull(sut);
    Assert.Equal(version, sut.ToString());
  }

  [Theory]
  [InlineData("12.74.2-PullRequest226923.10+Branch.pull-221483-merge.Sha.65650bba4fecd7bcab06347432d481a0f7c3c624")]
  [InlineData("12.74.2-PullRequest226923.10+65650bba4fecd7bcab06347432d481a0f7c3c624")]
  public void Ensure_pull_request_parses(string version)
  {
    var sut = SemanticVersionDescriptor.FromString(version);
    Assert.NotNull(sut);
    Assert.Equal(version, sut.ToString());
  }
}
