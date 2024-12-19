using CommonBytecode.Data.AnyValue;
using QuarkMap;

namespace QuarkStaticVariables;

public static class QuarkStaticVariables
{
    private static readonly QuarkMapImpl<Any, Any> _globals = new();

    public static void SetStatic(Any key, Any value) => _globals.Set(key, value);

    public static Any GetStatic(Any key) => _globals.Get(key);
}