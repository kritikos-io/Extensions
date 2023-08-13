namespace Kritikos.Extensions.Linq.Samples.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class IsOrderedBenchmarks
{
  private readonly IQueryable<Animal> animals
    = AnimalProvider.Animals.Generate(1000).AsQueryable();

  [Benchmark]
  public bool OrderedIsOrderedManual()
  {
    var buffer = animals
      .OrderBy(x => x.Id)
      .ToList();

    var previous = buffer[0];
    for (var i = 1; i < buffer.Count; i++)
    {
      var current = buffer[i];
      if (previous.Id.CompareTo(current.Id) > 0)
      {
        return false;
      }

      previous = current;
    }

    return true;
  }

  [Benchmark]
  public bool UnOrderedIsOrderedManual()
  {
    var buffer = animals
      .ToList();

    var previous = buffer[0];
    for (var i = 1; i < buffer.Count; i++)
    {
      var current = buffer[i];
      if (previous.Id.CompareTo(current.Id) > 0)
      {
        return false;
      }

      previous = current;
    }

    return true;
  }
}
