namespace Kritikos.Extensions.Version;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

public partial record SemanticVersionDescriptor
{
  private const RegexOptions ParserOption = RegexOptions.Compiled
                                            | RegexOptions.CultureInvariant
                                            | RegexOptions.ExplicitCapture
                                            | RegexOptions.Singleline;

  [StringSyntax(StringSyntaxAttribute.Regex)]
  private const string FullVersionIdentifier =
    @"^(?<Major>0|[1-9]\d*)\.(?<Minor>0|[1-9]\d*)\.(?<Patch>0|[1-9]\d*)(-(?<PreRelease>(0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(\.(0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(\+(Branch\.(?<Branch>.+)\.Sha\.)?(?<Sha>.+))?$";

  [StringSyntax(StringSyntaxAttribute.Regex)]
  private const string PreReleaseIdentifier = @"^((?<WorkItem>\d+)\.?\-?)?(.+)((\.)(?<Counter>\d+))$";

  private SemanticVersionDescriptor()
  {
  }

  public int Major { get; private init; }

  public int Minor { get; private init; }

  public int Patch { get; private init; }

  internal string Version { get; private init; } = string.Empty;

  public string PreReleaseTag { get; private init; } = string.Empty;

  public string WorkItem { get; private init; } = string.Empty;

  public int CommitCounter { get; private init; }

  public string Branch { get; private init; } = string.Empty;

  public string Sha1 { get; private init; } = string.Empty;

  public static SemanticVersionDescriptor FromAssembly(Assembly assembly)
    => FromInformationalVersionAttribute(assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!);

  public static SemanticVersionDescriptor FromInformationalVersionAttribute(AssemblyInformationalVersionAttribute attr)
    => FromString(attr.InformationalVersion);

  /// <inheritdoc />
  public override string ToString()
  {
    var stringBuilder = new StringBuilder(3);
    stringBuilder.Append(Version);
    if (!string.IsNullOrWhiteSpace(PreReleaseTag))
    {
      stringBuilder.Append(CultureInfo.InvariantCulture, $"-{PreReleaseTag}");
    }

    if (!string.IsNullOrWhiteSpace(Branch))
    {
      stringBuilder.Append(CultureInfo.InvariantCulture, $"+Branch.{Branch}.Sha.{Sha1}");
    }

    if (!string.IsNullOrWhiteSpace(Sha1) && string.IsNullOrWhiteSpace(Branch))
    {
      stringBuilder.Append(CultureInfo.InvariantCulture, $"+{Sha1}");
    }

    return stringBuilder.ToString();
  }

  internal static SemanticVersionDescriptor FromString(string version)
  {
    if (string.IsNullOrWhiteSpace(version))
    {
      throw new ArgumentNullException(nameof(version), "Version can not be null or whitespace");
    }

    var match = FullVersionParser().Match(version);
    if (!match.Success)
    {
      throw new ArgumentException("Malformed semantic version string", nameof(version));
    }

    var major = int.Parse(match.Groups["Major"].Value, CultureInfo.InvariantCulture);
    var minor = int.Parse(match.Groups["Minor"].Value, CultureInfo.InvariantCulture);
    var patch = int.Parse(match.Groups["Patch"].Value, CultureInfo.InvariantCulture);
    var prerelease = PreReleaseParser().Match(match.Groups["PreRelease"].Value);

    var result = new SemanticVersionDescriptor()
    {
      Major = major,
      Minor = minor,
      Patch = patch,
      Version = $"{major}.{minor}.{patch}",
      PreReleaseTag = match.Groups["PreRelease"].Value,
      Branch = match.Groups["Branch"].Value,
      Sha1 = match.Groups["Sha"].Value,
      WorkItem = prerelease.Groups["WorkItem"].Value,
      CommitCounter = prerelease.Success
        ? int.Parse(prerelease.Groups["Counter"].Value, CultureInfo.InvariantCulture)
        : 0,
    };
    return result;
  }

  [GeneratedRegex(FullVersionIdentifier, ParserOption)]
  private static partial Regex FullVersionParser();

  [GeneratedRegex(PreReleaseIdentifier, ParserOption)]
  private static partial Regex PreReleaseParser();
}
