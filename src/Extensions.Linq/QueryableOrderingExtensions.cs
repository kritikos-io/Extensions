#pragma warning disable SA1625
namespace Kritikos.Extensions.Linq;

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

public static class QueryableOrderingExtensions
{
  /// <summary>
  /// Sorts the elements of a sequence by <paramref name="property"/> in ascending order.
  /// </summary>
  /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
  /// <param name="source">A sequence of values to order.</param>
  /// <param name="property">The name of the property to use in ordering.</param>
  /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted according to <paramref name="property"/> in ascending order.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="property"/> does not exist on <typeparamref name="TSource"/> or is empty.</exception>
  public static IOrderedQueryable<TSource> OrderByProperty<TSource>(
    this IQueryable<TSource> source,
    string property)
    => source.Order(ListSortDirection.Ascending, property, false);

  /// <summary>
  /// Performs a subsequent ordering of the elements in a sequence in ascending order.
  /// </summary>
  /// <param name="source">The type of the elements of <paramref name="source"/>.</param>
  /// <param name="property">The name of the property to use in ordering.</param>
  /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
  /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted according to <paramref name="property"/> in ascending order.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="property"/> does not exist on <typeparamref name="TSource"/> or is empty.</exception>
  public static IOrderedQueryable<TSource> ThenByProperty<TSource>(
    this IOrderedQueryable<TSource> source,
    string property)
    => source.Order(ListSortDirection.Ascending, property, true);

  /// <summary>
  /// Sorts the elements of a sequence by <paramref name="property"/> in descending order.
  /// </summary>
  /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
  /// <param name="source">A sequence of values to order.</param>
  /// <param name="property">The name of the property to use in ordering.</param>
  /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted according to <paramref name="property"/>in descending order.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="property"/> does not exist on <typeparamref name="TSource"/> or is empty.</exception>
  public static IOrderedQueryable<TSource> OrderByPropertyDescending<TSource>(
    this IQueryable<TSource> source,
    string property)
    => source.Order(ListSortDirection.Descending, property, false);

  /// <summary>
  /// Performs a subsequent ordering of the elements in a sequence in descending order.
  /// </summary>
  /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
  /// <param name="source">A sequence of values to order.</param>
  /// <param name="property">The name of the property to use in ordering.</param>
  /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are sorted according to <paramref name="property"/>in descending order.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="property"/> does not exist on <typeparamref name="TSource"/> or is empty.</exception>
  public static IOrderedQueryable<TSource> ThenByPropertyDescending<TSource>(
    this IOrderedQueryable<TSource> source,
    string property)
    => source.Order(ListSortDirection.Descending, property, true);

  [ExcludeFromCodeCoverage]
  private static IOrderedQueryable<TSource> Order<TSource>(
    this IQueryable<TSource> source,
    ListSortDirection direction,
    string propertyName,
    bool subsequent)
  {
    var entity = typeof(TSource);
    var property = entity.GetProperty(propertyName);

    if (subsequent && source is not IOrderedQueryable<TSource>)
    {
      throw new ArgumentException($"{nameof(source)} should be ordered before trying subsequent ordering!");
    }

    if (property == null)
    {
      throw new ArgumentException($"Type of {entity.FullName} does not contain property {propertyName}!");
    }

    var arg = Expression.Parameter(entity, "x");
    var body = Expression.Property(arg, propertyName);

    dynamic selector = Expression.Lambda(body, arg);
    return subsequent
      ? direction == ListSortDirection.Ascending
        ? Queryable.ThenBy(source as IOrderedQueryable<TSource>, selector)
        : Queryable.ThenByDescending(source as IOrderedQueryable<TSource>, selector)
      : direction == ListSortDirection.Ascending
        ? Queryable.OrderBy(source, selector)
        : Queryable.OrderByDescending(source, selector);
  }
}
