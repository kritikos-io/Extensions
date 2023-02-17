namespace Kritikos.Extensions.LinqTests;

using System;
using System.Linq;

using Kritikos.Extensions.Linq;
using Kritikos.Extensions.Linq.Samples;

using Xunit;

public class OrderedQueryableTests
{
  private static IQueryable<Animal> Animals { get; } = AnimalProvider.Animals
    .Generate(30)
    .AsQueryable();

  [Fact]
  public void Null_exception_is_propagated()
  {
    IOrderedQueryable<Animal> query = null!;

    Assert.Throws<ArgumentNullException>(() => query.Slice(1, 1));
    Assert.Throws<ArgumentNullException>(() => query.OrderByProperty("Id"));
  }

  [Fact]
  public void Slice_should_throw_exception_on_negative_page_size()
  {
    var query = Animals.OrderBy(x => x.Id);

    Assert.Throws<ArgumentException>(() => query.Slice(1, -5));
  }

  [Fact]
  public void Slice_should_throw_exception_on_negative_page_number()
  {
    var query = Animals.OrderBy(x => x.Id);

    Assert.Throws<ArgumentException>(() => query.Slice(-1, 56));
  }

  [Fact]
  public void Slice_should_throw_on_size_zero_and_page_number_greater_than_one()
  {
    var query = Animals.OrderBy(x => x.Id);

    Assert.Throws<ArgumentException>(() => query.Slice(2, 0));
  }

  [Theory]
  [InlineData(25, 1)]
  [InlineData(4, 10)]
  [InlineData(2, 30)]
  [InlineData(90, 40)]
  public void Slice_returns_proper_element_count(int page, int size)
  {
    var query = Animals.OrderBy(x => x.Id)
      .Slice(page, size)
      .ToList();

    Assert.True(size >= query.Count);
  }

  [Fact]
  public void OrderByProperty_should_throw_exception_on_non_existing_property_name()
  {
    var query = Animals;

    Assert.Throws<ArgumentException>(() => query.OrderByProperty("Blah"));
  }

  [Fact]
  public void OrderByProperty_has_same_results_as_OrderBy()
  {
    var query = Animals;
    var ordered = query.OrderBy(x => x.Id);
    var orderedByProperty = query.OrderByProperty("Id");

    Assert.Equal(ordered, orderedByProperty);
  }

  [Fact]
  public void OrderByPropertyDescending_has_same_results_as_OrderByDescending()
  {
    var query = Animals;
    var ordered = query.OrderByDescending(x => x.Id);
    var orderedByProperty = query.OrderByPropertyDescending("Id");

    Assert.Equal(ordered, orderedByProperty);
  }

  [Fact]
  public void ThenByPropertyDescending_has_same_results_as_ThenByDescending()
  {
    var query = Animals.OrderBy(x => x.Id);
    var ordered = query.ThenByDescending(x => x.Name);
    var orderedByProperty = query.ThenByPropertyDescending("Name");

    Assert.Equal(ordered, orderedByProperty);
  }

  [Fact]
  public void ThenByProperty_has_same_results_as_ThenBy()
  {
    var query = Animals.OrderBy(x => x.Id);
    var ordered = query.ThenBy(x => x.Name);
    var orderedByProperty = query.ThenByProperty("Name");

    Assert.Equal(ordered, orderedByProperty);
  }

  [Fact]
  public void Pagination_should_pick_proper_elements()
  {
    var query = Enumerable.Range(1, 100).AsQueryable().OrderBy(x => x);

    var expected = Enumerable.Range(21, 10).ToList();
    var page = query.ToPagedResult(3, 10);

    Assert.Equal(expected, page.Items);
    Assert.Equal(100, page.TotalCount);
    Assert.Equal(10, page.TotalPages);
    Assert.Equal(10, page.PageSize);
    Assert.Equal(3, page.PageNumber);
    Assert.True(page.HasNextPage);
    Assert.True(page.HasPreviousPage);
  }

  [Fact]
  public void Pagination_last_page_should_work()
  {
    var query = Enumerable.Range(1, 100).AsQueryable().OrderBy(x => x);

    var page = query.ToPagedResult(12, 9);

    Assert.Equal(100, page.Items.Single());
    Assert.Equal(100, page.TotalCount);
    Assert.Equal(9, page.PageSize);
    Assert.Equal(12, page.PageNumber);
    Assert.False(page.HasNextPage);
    Assert.True(page.HasPreviousPage);
  }

  [Fact]
  public void Pagination_enumeration_should_work()
  {
    var query = Enumerable.Range(1, 100).AsQueryable().OrderBy(x => x);

    var expected = query.Skip(20).Take(10).ToList();
    var page = query.ToPagedResult(3, 10);

    var count = 0;
    foreach (var item in page)
    {
      Assert.Equal(expected[count], item);
      count++;
    }

    Assert.Equal(expected.Count, count);
  }
}
