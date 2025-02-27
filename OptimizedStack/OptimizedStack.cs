using CommonDataStructures;

namespace OptimizedStack;

public class OptimizedStack<T>(int size = 1024) : IReadOnlyStack<T>
{
    private readonly T[] _stack = new T[size];

    private int _count;

    public T Get(int ind) => ind > 0 ? _stack[ind] : _stack[_count - ind];

    public void Push(T value)
    {
        _stack[_count++] = value;
    }

    public T Pop()
    {
        var value = _stack[--_count];
        _stack[_count] = default!;
        return value;
    }
}