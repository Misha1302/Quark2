namespace CommonExtensions;

public static class DictionaryExtensions
{
    public static void CreateNewOrAdd<TKey, TListValue>(
        this IDictionary<TKey, List<TListValue>> d,
        TKey k,
        TListValue v
    ) where TKey : notnull
    {
        if (!d.ContainsKey(k))
            d.Add(k, []);

        d[k].Add(v);
    }

    public static void CreateOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> d,
        TKey k,
        TValue v
    ) where TKey : notnull
    {
        if (!d.ContainsKey(k))
            d.Add(k, default!);

        d[k] = v;
    }
}