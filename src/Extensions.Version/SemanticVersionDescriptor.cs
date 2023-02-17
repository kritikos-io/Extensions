namespace Kritikos.Extensions.Version;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

public partial class SemanticVersionDescriptor
{
  private const RegexOptions ParserOption = RegexOptions.CultureInvariant;

  // RegexOptions.Singleline
  // | RegexOptions.ExplicitCapture;
  [StringSyntax(StringSyntaxAttribute.Regex)]
  private const string FullVersionIdentifier =
    @"^(?<Major>[0-9]|[1-9][0-9]+)\.(?<Minor>[0-9]|[1-9][0-9]+)\.(?<Patch>[0-9]|[1-9][0-9]+)(\-(?<PreRelease>[\w\-\.]*))?(\+(Branch\.(?<Branch>.+?)\.Sha\.)?(?<Sha>.+))?$";

  private SemanticVersionDescriptor()
  {
  }

  public string Version { get; private set; } = string.Empty;

  public string PreReleaseTag { get; private set; } = string.Empty;

  public string Branch { get; private set; } = string.Empty;

  public string Sha1 { get; private set; } = string.Empty;

  public int Major { get; private set; }

  public int Minor { get; private set; }

  public int Patch { get; private set; }

  public static SemanticVersionDescriptor FromAssembly(Assembly assembly)
    => FromInformationalVersionAttribute(assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!);

  public static SemanticVersionDescriptor FromInformationalVersionAttribute(AssemblyInformationalVersionAttribute attr)
    => FromString(attr?.InformationalVersion!);

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

    var minor = 0;
    var patch = 0;

    var isValid = int.TryParse(match.Groups["Major"].Value, out var major)
                  && int.TryParse(match.Groups["Minor"].Value, out minor)
                  && int.TryParse(match.Groups["Patch"].Value, out patch);

    if (!isValid)
    {
      throw new ArgumentException("Could not parse Major, Minor, Patch segments of semantic version", nameof(version));
    }

    var result = new SemanticVersionDescriptor()
    {
      Major = major,
      Minor = minor,
      Patch = patch,
      Version = $"{major}.{minor}.{patch}",
      PreReleaseTag = match.Groups["PreRelease"].Value,
      Branch = match.Groups["Branch"].Value,
      Sha1 = match.Groups["Sha"].Value,
    };
    return result;
  }

  [GeneratedRegex(FullVersionIdentifier, ParserOption)]
  private static partial Regex FullVersionParser();
}
