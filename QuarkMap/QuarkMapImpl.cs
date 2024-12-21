using System.Collections;
using CommonBytecode.Data.AnyValue;

namespace QuarkMap;

public class QuarkMapImpl<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary =
        typeof(TKey) == typeof(Any)
            ? new Dictionary<TKey, TValue>((IEqualityComparer<TKey>?)AnyEqualityComparer.Instance)
            : new Dictionary<TKey, TValue>();

    public int Count => _dictionary.Count;

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() =>
        _dictionary.GetEnumerator();

    public IEnumerator GetEnumerator() => _dictionary.GetEnumerator();

    public TValue Get(TKey key) => _dictionary[key];

    public void Set(TKey key, TValue value) => _dictionary[key] = value;

    public override string ToString()
    {
        return string.Join("\n", _dictionary.Select(x => x.Key + ": " + x.Value));
    }

    public void Remove(TKey key) => _dictionary.Remove(key);

    public bool HasKey(TKey key) => _dictionary.ContainsKey(key);
}