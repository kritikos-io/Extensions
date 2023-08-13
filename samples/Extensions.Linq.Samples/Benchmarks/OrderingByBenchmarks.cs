namespace Kritikos.Extensions.Linq.Samples.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class OrderingByBenchmarks
{
  private readonly IQueryable<Animal> animals
    = AnimalProvider.Animals.Generate(1000).AsQueryable();

  [Benchmark]
  public void OrderByPropertyName()
  {
    var order = animals.OrderByProperty(nameof(Animal.Name));
  }

  [Benchmark]
  public void OrderBy()
  {
    var order = animals.OrderBy(x => x.Name);
  }
}
