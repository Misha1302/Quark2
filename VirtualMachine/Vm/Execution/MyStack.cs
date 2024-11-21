namespace VirtualMachine.Vm.Execution;

public class MyStack<T>
{
    private readonly List<T> _list = [];

    public int Count => _list.Count;

    public void Push(T value)
    {
        _list.Add(value);
    }

    public T Pop()
    {
        var r = _list[^1];
        // TODO: optimize
        _list.RemoveAt(_list.Count - 1);
        return r;
    }

    public T Get(Index ind) => _list[ind];

    public void DropMany(long argsCount)
    {
        // TODO: optimize
        for (var i = 0; i < argsCount; i++) Pop();
    }

    public override string ToString() => string.Join(", ", _list);

    public void PushMany(List<T> vmValues)
    {
        _list.AddRange(vmValues);
    }
}