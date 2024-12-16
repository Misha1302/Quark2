using CommonBytecode.Data.AnyValue;

namespace QuarkStrings;

public static class StringManipulations
{
    public static Any Concat(Any a, Any b) => a.Get<string>() + b.Get<string>();
}