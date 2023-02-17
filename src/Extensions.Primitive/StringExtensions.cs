namespace Kritikos.Extensions.Primitive;

using System.Diagnostics.CodeAnalysis;

public static class StringExtensions
{
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
}
