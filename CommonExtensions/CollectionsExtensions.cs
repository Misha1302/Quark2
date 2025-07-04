namespace CommonExtensions;

public static class CollectionsExtensions
{
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection)
            action(item);
    }

    public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => enumerable.Any();

    public static bool IsEmpty<T>(this List<T> enumerable) => enumerable.Count != 0;

    public static bool IsEmpty<T>(this T[] enumerable) => enumerable.Length != 0;

    public static bool IsEmpty<T>(this ICollection<T> enumerable) => enumerable.Count != 0;
}