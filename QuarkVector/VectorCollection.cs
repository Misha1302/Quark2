using System.Runtime.InteropServices;

namespace QuarkVector;

public class VectorCollection<T>
{
    private readonly List<T> _data = [];

    public int Count => _data.Count;

    public T this[Index index]
    {
        get => CollectionsMarshal.AsSpan(_data)[index];
        set => CollectionsMarshal.AsSpan(_data)[index] = value;
    }

    public void Add(T item)
    {
        _data.Add(item);
    }

    public void RemoveAt(int index)
    {
        _data.RemoveAt(index);
    }

    public void RemoveLast(int index)
    {
        _data.RemoveAt(_data.Count - 1);
    }

    public void SetSize(long length)
    {
        if (_data.Count < length)
            CollectionsMarshal.SetCount(_data, (int)length);
    }

    public override string ToString() => string.Join(", ", _data);
}