namespace VirtualMachine.Vm.Execution;

public class MyStack<T>(int maxSize)
{
    private readonly T[] _data = new T[maxSize];

    public int Count { get; private set; }

    public void Push(T value)
    {
        _data[Count++] = value;
    }

    public T Pop()
    {
        Count--;
        Throw.Assert(Count >= 0);
        return _data[Count];
    }

    public T Get(int ind) => _data[ind > 0 ? ind : Count + ind];

    public void DropMany(long argsCount)
    {
        Count -= (int)argsCount;
        Throw.Assert(Count >= 0);
    }

    public override string ToString() => string.Join(", ", _data);

    public void PushMany(Span<T> vmValues)
    {
        foreach (var vmValue in vmValues)
            Push(vmValue);
    }
}