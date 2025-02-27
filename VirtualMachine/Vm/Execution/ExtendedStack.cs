namespace VirtualMachine.Vm.Execution;

public class ExtendedStack<T>(int maxSize) : IReadOnlyStack<T>
{
    private readonly T[] _data = new T[maxSize];

    public int Count { get; private set; }

    public T Get(int ind)
    {
        var ind2 = ind > 0 ? ind : Count + ind;
        Throw.AssertDebug(ind2 >= 0 && ind2 < Count);
        return _data[ind2];
    }

    public void Push(T value)
    {
        Throw.AssertDebug(Count < _data.Length, "Stack is full");
        _data[Count++] = value;
    }

    public T Pop()
    {
        Count--;
        Throw.AssertDebug(Count >= 0);
        return _data[Count];
    }

    public void DropMany(long argsCount)
    {
        Count -= (int)argsCount;
        Throw.AssertDebug(Count >= 0);
    }

    public override string ToString()
    {
        try
        {
            //return string.Join(", ", _data);
            return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    public void PushMany(Span<T> vmValues)
    {
        foreach (var vmValue in vmValues)
            Push(vmValue);
    }

    public void Clear()
    {
        Count = 0;
    }
}