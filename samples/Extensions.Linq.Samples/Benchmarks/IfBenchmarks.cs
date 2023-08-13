namespace Kritikos.Extensions.Linq.Samples.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

[MemoryDiagnoser]
[ThreadingDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class IfBenchmarks
{
  private readonly IQueryable<Animal> animals
    = AnimalProvider.Animals.Generate(1000).AsQueryable();

  [Params(true, false)]
  public bool Condition { get; set; }

  [Benchmark]
  public List<Animal> ByTakeIfExtension()
  {
    return animals.TakeIf(Condition, 500).ToList();
  }

  [Benchmark]
  public List<Animal> ByEvaluatingCondition()
  {
    if (Condition)
    {
      return animals.Take(500).ToList();
    }

    return animals.ToList();
  }
}
