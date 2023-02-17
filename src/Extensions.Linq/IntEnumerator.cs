namespace Kritikos.Extensions.Linq;

public ref struct IntEnumerator
{
  private readonly int end;
  private int current;

  public IntEnumerator(Range range)
  {
    if (range.End.IsFromEnd)
    {
      throw new NotSupportedException("Cannot enumerate from end");
    }

    current = range.Start.Value - 1;
    end = range.End.Value;
  }

  public int Current => current;

  public bool MoveNext()
  {
    current++;
    return current <= end;
  }
}
