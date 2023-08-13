// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Kritikos.Extensions.Version;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSemanticVersionDescriptor(this IServiceCollection services, Assembly assembly)
    => services.AddSingleton(SemanticVersionDescriptor.FromAssembly(assembly));

  public static IServiceCollection AddSemanticVersionDescriptor<TAssembly>(this IServiceCollection services)
    => services.AddSingleton(SemanticVersionDescriptor.FromAssembly(typeof(TAssembly).Assembly));
}
