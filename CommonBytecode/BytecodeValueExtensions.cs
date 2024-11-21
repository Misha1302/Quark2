using System.Globalization;
using static CommonBytecode.BytecodeValueType;

namespace CommonBytecode;

public static class BytecodeValueExtensions
{
    public static string ToStringExtension(this BytecodeValue value) => ToStringValue(value.Value, value.Type);

    public static string ToStringValue(Any value, BytecodeValueType type)
    {
        return type switch
        {
            Nil => "Nil",
            Number => value.Get<double>().ToString(CultureInfo.InvariantCulture),
            Str => value.Get<string>(),
            Map => Throw.InvalidOpEx<string>(),
            List => string.Join(", ", value.Get<List<BytecodeValue>>()),
            VmFunction => value.Get<BytecodeFunction>().Name,
            SharpFunctionAddress => value.Get<nint>().ToString("X"),
            NativeI64 => $"n_{value.Get<long>()}",
            BytecodeValueType.Any => $"any: {value.Get<long>()}",
            _ => Throw.InvalidOpEx<string>(),
        };
    }
}