namespace SharpAnyType;

public static class AnyValueTypeExtensions
{
    public static bool HasFlagFast(this AnyValueType value, AnyValueType flag) => (value & flag) != 0;

    public static bool IsRefType(this AnyValueType value) =>
        ((Str | SomeSharpObject) & value) != 0;
}