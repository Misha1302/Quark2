using CommonBytecode.Data.AnyValue;

namespace QuarkMap;

public class QuarkMapImpl<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary =
        typeof(TKey) == typeof(Any)
            ? new Dictionary<TKey, TValue>((IEqualityComparer<TKey>?)Any.Nil)
            : new Dictionary<TKey, TValue>();

    public TValue Get(TKey key)
    {
        var q = (Any)(object)_dictionary.First().Key;
        var w = (string)q.Value == (string)((Any)(object)key).Value;
        return _dictionary[key];
    }

    public void Set(TKey key, TValue value) => _dictionary[key] = value;
}