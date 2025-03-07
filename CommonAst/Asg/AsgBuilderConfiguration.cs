namespace DefaultAstImpl.Asg;

public record AsgBuilderConfiguration<T>(SortedDictionary<float, List<INodeCreator<T>>> CreatorLevels);

public static class SortedDictionaryExtensions
{
    public static void CreateNewOrAdd<TKey, TListValue>(
        this SortedDictionary<TKey, List<TListValue>> d,
        TKey k,
        TListValue v
    ) where TKey : notnull
    {
        d.TryAdd(k, []);
        d[k].Add(v);
    }
}