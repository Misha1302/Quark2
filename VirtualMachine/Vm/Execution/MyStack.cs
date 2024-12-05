namespace VirtualMachine.Vm.Execution;

public class MyStack<T>(int maxSize)
{
    private readonly T[] _data = new T[maxSize];
    private int _index;

    public int Count => _data.Length;

    public void Push(T value)
    {
        _data[_index++] = value;
    }

    public T Pop()
    {
        _index--;
        Throw.Assert(_index >= 0);
        return _data[_index];
    }

    public T Get(int ind) => _data[ind > 0 ? ind : _index + ind];

    public void DropMany(long argsCount)
    {
        _index -= (int)argsCount;
        Throw.Assert(_index >= 0);
    }

    public override string ToString() => string.Join(", ", _data);

    public void PushMany(Span<T> vmValues)
    {
        foreach (var vmValue in vmValues)
            Push(vmValue);
    }
}