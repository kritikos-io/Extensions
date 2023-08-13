namespace Kritikos.Extensions.Linq.Samples.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class ShufflingBenchmarks
{
  private readonly IQueryable<Animal> animals
    = AnimalProvider.Animals.Generate(1000).AsQueryable();

  [Benchmark]
  public List<Animal> ByRandomIndex()
  {
    var result = animals
      .OrderBy(x => Random.Shared.Next(0, 1000))
      .ToList();

    return result;
  }
}
