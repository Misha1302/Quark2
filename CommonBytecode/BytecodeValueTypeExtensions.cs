using static CommonBytecode.BytecodeValueType;

namespace CommonBytecode;

public static class BytecodeValueTypeExtensions
{
    public static bool HasFlagFast(this BytecodeValueType value, BytecodeValueType flag) => (value & flag) != 0;

    public static bool IsValueType(this BytecodeValueType value) =>
        ((Number | NativeI64 | Nil | SharpFunctionAddress) & value) != 0;

    public static bool IsRefType(this BytecodeValueType value) =>
        ((Str | Map | List | VmFunction) & value) != 0;
}