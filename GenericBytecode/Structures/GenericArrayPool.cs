namespace GenericBytecode.Structures;

public class GenericArrayPool<T>
{
    public static readonly GenericArrayPool<T> Shared = new();

    public readonly Dictionary<int, List<T[]>> Arrays = [];

    public T?[] Rent(int len)
    {
        if (!Arrays.ContainsKey(len))
            Arrays.Add(len, []);

        if (Arrays[len].Count == 0)
            Arrays[len].Add(new T[len]);

        var arr = Arrays[len][^1];
        Arrays[len].RemoveAt(Arrays[len].Count - 1);
        return arr;
    }

    public void Return(T[] args)
    {
        Arrays[args.Length].Add(args);
    }
}