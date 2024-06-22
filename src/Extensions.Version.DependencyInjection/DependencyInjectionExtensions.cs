// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Kritikos.Extensions.Version;

using Microsoft.Extensions.DependencyInjection.Extensions;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddSemanticVersionDescriptor(this IServiceCollection services, Assembly assembly)
    => services.AddSingleton(SemanticVersionDescriptor.FromAssembly(assembly));

  public static IServiceCollection AddSemanticVersionDescriptor<TAssembly>(this IServiceCollection services)
    => services.AddSemanticVersionDescriptor(typeof(TAssembly).Assembly);

  public static void TryAddSemanticVersionDescriptor(this IServiceCollection services, Assembly assembly)
    => services.TryAddSingleton(SemanticVersionDescriptor.FromAssembly(assembly));

  public static void TryAddSemanticVersionDescriptor<TAssembly>(this IServiceCollection services)
    => services.TryAddSemanticVersionDescriptor(typeof(TAssembly).Assembly);
}
