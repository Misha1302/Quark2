using System.Diagnostics.CodeAnalysis;
using System.Text;
using QuarkMap;
using SharpAnyType;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace QuarkSerializaton;

public static class QuarkSerializer
{
    public static Any DeserializeIntoMap(Any json)
    {
        if (!TryDeserialize<Dictionary<string, string>>(json, out var values))
            return Any.Nil;

        var map = new QuarkMapImpl<Any, Any>();
        foreach (var value in values)
            map.Set(value.Key, value.Value);

        return map.ObjectToAny(AnyValueType.SomeSharpObject);
    }

    public static Any DeserializeIntoMapOfMaps(Any json)
    {
        if (!TryDeserialize<Dictionary<string, Dictionary<string, string>>>(json, out var values))
            return Any.Nil;

        var map = new QuarkMapImpl<Any, Any>();
        foreach (var value in values)
        {
            var map2 = new QuarkMapImpl<Any, Any>();
            map.Set(value.Key, map2.ObjectToAny(AnyValueType.SomeSharpObject));
            foreach (var pair in value.Value)
                map2.Set(pair.Key, pair.Value);
        }

        return map.ObjectToAny(AnyValueType.SomeSharpObject);
    }

    private static bool TryDeserialize<T>(Any json, [NotNullWhen(true)] out T? values)
    {
        try
        {
            values = JsonSerializer.Deserialize<T>(json.Get<string>());
            return values != null;
        }
        catch
        {
            values = default;
            return false;
        }
    }

    public static Any SerializeMap(Any json)
    {
        var sb = new StringBuilder();
        var map = json.Get<QuarkMapImpl<Any, Any>>();
        sb.Append('{');

        foreach (var pair in (IEnumerable<KeyValuePair<Any, Any>>)map)
            sb.Append(
                $"\"{pair.Key}\":{(pair.Value.Value is QuarkMapImpl<Any, Any> ? SerializeMap(pair.Value) : '"' + pair.Value.ToString() + '"')},");

        if (map.Count != 0)
            sb.Remove(sb.Length - 1, 1);
        sb.Append('}');
        return sb.ToString();
    }
}