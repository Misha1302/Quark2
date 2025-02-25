namespace CommonBytecode.Data.AnyValue;

public static class BytecodeValueTypeExtensions
{
    public static bool IsRefType(this BytecodeValueType value) =>
        ((Str | SomeSharpObject) & value) != 0;
}