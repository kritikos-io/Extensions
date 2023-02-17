namespace Kritikos.Extensions.Linq.Samples;

using Bogus;

public static class AnimalProvider
{
  public static Faker<Animal> Animals { get; }
    = new Faker<Animal>()
      .RuleFor(e => e.Id, _ => Guid.NewGuid())
      .RuleFor(e => e.Name, f => f.Name.FirstName());
}
