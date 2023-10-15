namespace Kritikos.Extensions.Primitive;

public static class TypeExtensions
{
  public static IEnumerable<Type> GetParentTypes(this Type? type)
  {
    if (type is null)
    {
      yield break;
    }

    foreach (var i in type.GetInterfaces())
    {
      yield return i;
    }

    var currentBaseType = type.BaseType;
    while (currentBaseType is not null)
    {
      yield return currentBaseType;
      currentBaseType = currentBaseType.BaseType;
    }
  }
}
