namespace Kritikos.Extensions.Primitive;

using System.Diagnostics.CodeAnalysis;

public static class StringExtensions
{
  /// <summary>
  /// The Winkler modification will not be applied unless the
  /// percent match was at or above the mWeightThreshold percent
  /// without the modification.
  /// </summary>
  /// <remarks>
  /// Winkler's paper used a default value of 0.7.
  /// </remarks>
  public const double WinklerWeightThreshold = 0.7;

  /// <summary>
  /// Size of the prefix to be concidered by the Winkler modification.
  /// </summary>
  /// <remarks>
  /// Winkler's paper used a default value of 4.
  /// </remarks>
  public const int WinklerPrefixSize = 4;

  /// <summary>
  /// Indicates whether a specified string is <see langword="null"/>, <see cref="string.Empty"/>, or consists only of white-space characters.
  /// </summary>
  /// <param name="value">The string to test.</param>
  /// <returns>
  /// <see langword="true" /> if the <paramref name="value"/> parameter is <see langword="null"/> or <see cref="string.Empty"/>, or if value consists exclusively of white-space characters; otherwise, <see langword="false"/>.
  /// </returns>
  /// <seealso cref="IsNullOrEmpty"/>
  [ExcludeFromCodeCoverage]
  public static bool IsNullOrWhiteSpace(this string value)
    => string.IsNullOrWhiteSpace(value);

  /// <summary>
  /// Indicates whether the specified string is <see langword="null"/> or an <see cref="string.Empty"/> string ("").
  /// </summary>
  /// <param name="value">The string to test.</param>
  /// <returns><see langword="true" /> if the <paramref name="value"/> parameter is <see langword="null"/> or an <see cref="string.Empty"/> string (""); otherwise, <see langword="false"/>.</returns>
  /// <seealso cref="IsNullOrWhiteSpace"/>
  [ExcludeFromCodeCoverage]
  public static bool IsNullOrEmpty(this string value)
    => string.IsNullOrEmpty(value);

  /// <summary>
  /// Calculates the Jaro-Winkler distance between the specified
  /// strings. The distance is symmetric and will fall in the
  /// range 0 (perfect match) to 1 (no match).
  /// </summary>
  /// <param name="source">First <see cref="string"/>.</param>
  /// <param name="target">Second <see cref="string"/>.</param>
  /// <param name="winklerPrefixSize">Length of the prefix to be concidered by the Winkler modification.</param>
  /// <param name="winklerWeightThreshold">The Winkler modification will not be applied unless the percent match was at or above the Weight Threshold percent without the modification.</param>
  /// <remarks>On longer strings or multiple words, refer to <see cref="StringExtensions.DamerauLevenshteinDistance"/>.
  /// Provided the two strings start with matching <paramref name="winklerPrefixSize"/> characters, they are rewarded if they were at least <paramref name="winklerWeightThreshold"/> similar,
  /// further decreasing their distance.
  /// Note that similarity is calculated as 1 - distance, so the returned value is the inverse of the distance.
  /// </remarks>
  /// <returns>A number in the range 0 (strings are equal) to 1 (strings have no similarity).</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="source"/> or <paramref name="target"/> are null.</exception>
  public static double JaroWinklerDistance(
    this string source,
    string target,
    int winklerPrefixSize = WinklerPrefixSize,
    double winklerWeightThreshold = WinklerWeightThreshold)
  {
    ArgumentNullException.ThrowIfNull(source);
    ArgumentNullException.ThrowIfNull(target);

    if (source.IsNullOrEmpty() || target.IsNullOrEmpty())
    {
      return source == target
        ? 0.0
        : 1.0;
    }

    var searchRange = Math.Max(0, (Math.Max(source.Length, target.Length) / 2) - 1);

    var matchedSource = new bool[source.Length];
    var matchedTarget = new bool[target.Length];

    var common = 0;
    for (var i = 0; i < source.Length; ++i)
    {
      var start = Math.Max(0, i - searchRange);
      var end = Math.Min(i + searchRange + 1, target.Length);

      for (var j = start; j < end; ++j)
      {
        if (matchedTarget[j])
        {
          continue;
        }

        if (source[i] != target[j])
        {
          continue;
        }

        matchedSource[i] = true;
        matchedTarget[j] = true;
        ++common;
        break;
      }
    }

    if (common == 0)
    {
      return 1.0;
    }

    var transposed = 0;
    var k = 0;
    for (var i = 0; i < source.Length; ++i)
    {
      if (!matchedSource[i])
      {
        continue;
      }

      while (!matchedTarget[k])
      {
        ++k;
      }

      if (source[i] != target[k])
      {
        ++transposed;
      }

      ++k;
    }

    var halfTransposed = transposed / 2;
    double commonDouble = common;
    var weight = ((commonDouble / source.Length)
                  + (commonDouble / target.Length)
                  + ((common - halfTransposed) / commonDouble)) / 3.0;

    if (weight <= winklerWeightThreshold)
    {
      return 1 - weight;
    }

    var max = Math.Min(winklerPrefixSize, Math.Min(source.Length, target.Length));
    var pos = 0;
    while (pos < max && source[pos] == target[pos])
    {
      ++pos;
    }

    return 1 - (pos == 0
      ? weight
      : weight + (0.1 * pos * (1.0 - weight)));
  }

  /// <summary>
  /// Calculates the Damerau-Levenshtein distance between the specified strings.
  /// The distance ranges from 0 to the length of the longest string, increasing as strings are further apart.
  /// </summary>
  /// <param name="source">First <see cref="string"/>.</param>
  /// <param name="target">Second <see cref="string"/>.</param>
  /// <remarks>https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance,
  /// this method is more suitable for longer strings and/or spellcheckers, for string similarity see <see cref="JaroWinklerDistance"/>.</remarks>
  /// <returns>A number between 0 and the length of the longest string.</returns>
  public static int DamerauLevenshteinDistance(this string source, string target)
  {
    ArgumentNullException.ThrowIfNull(source);
    ArgumentNullException.ThrowIfNull(target);

    if (source.IsNullOrWhiteSpace())
    {
      return target.Length;
    }

    if (target.IsNullOrWhiteSpace())
    {
      return source.Length;
    }

    var dp = new int[source.Length + 1][];
    for (var i = 0; i <= source.Length; i++)
    {
      dp[i] = new int[target.Length + 1];
      dp[i][0] = i;
    }

    for (var i = 0; i <= target.Length; i++)
    {
      dp[0][i] = i;
    }

    for (var i = 1; i <= source.Length; i++)
    {
      for (var j = 1; j <= target.Length; j++)
      {
        var cost = source[i - 1] == target[j - 1]
          ? 0
          : 1;

        dp[i][j] = Math.Min(
          Math.Min(
            dp[i - 1][j] + 1, // Deletion
            dp[i][j - 1] + 1), // Insertion
          dp[i - 1][j - 1] + cost); // Substitution

        if (i > 1 && j > 1 && source[i - 1] == target[j - 2] && source[i - 2] == target[j - 1])
        {
          dp[i][j] = Math.Min(
            dp[i][j],
            dp[i - 2][j - 2] + cost); // Transposition
        }
      }
    }

    for (var i = 2; i <= source.Length; i++)
    {
      for (var j = 2; j <= target.Length; j++)
      {
        if (source[i - 1] == target[j - 1] && source[i - 2] == target[j - 2])
        {
          dp[i][j] = Math.Min(dp[i][j], dp[i - 2][j - 2] + 1); // Adjacent transposition
        }
      }
    }

    return dp[source.Length][target.Length];
  }
}
