namespace SharpAnyType;

/// <summary>
///     because we provide the ability to have complex types (for example [Number | Str] or just [Any]),
///     we need to provide the ability to combine VmValueType values
/// </summary>
[Flags]
public enum BytecodeValueType : long
{
    // every enum value is the power of two. To have an ability to mix types we need to take every bit by order 
    Nil = 1 << 0,
    Number = 1 << 1,
    Str = 1 << 2,
    SomeSharpObject = 1 << 3,
    NativeI64 = 1 << 4,

    // long.MaxValue - 0b111111111111111111111111111111111111111111111111111111111111111
    // just 63 enabled bits. It means that Any contains any type
    Any = long.MaxValue,
}