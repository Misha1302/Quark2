using System.Text;
using System.Text.Json;
using CommonBytecode.Data.AnyValue;
using ExceptionsManager;
using QuarkMap;

namespace QuarkSerializaton;

public static class QuarkSerializer
{
    public static Any DeserializeIntoMap(Any json)
    {
        var values = JsonSerializer.Deserialize<Dictionary<string, string>>(json.Get<string>());
        Throw.Assert(values != null, nameof(values) + " != null");

        var map = new QuarkMapImpl<Any, Any>();
        foreach (var value in values)
            map.Set(value.Key, value.Value);
        return map.ToAny(BytecodeValueType.SomeSharpObject);
    }

    public static Any SerializeMap(Any json)
    {
        var sb = new StringBuilder();
        var map = json.Get<QuarkMapImpl<Any, Any>>();
        sb.Append('{');

        foreach (var pair in (IEnumerable<KeyValuePair<Any, Any>>)map)
            sb.Append(
                $"{pair.Key}:{(pair.Value.Value is QuarkMapImpl<Any, Any> ? SerializeMap(pair.Value) : pair.Value)},");

        sb.Remove(sb.Length - 1, 1);
        sb.Append('}');
        return sb.ToString();
    }
}