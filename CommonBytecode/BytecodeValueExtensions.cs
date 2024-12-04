using System.Globalization;
using System.Runtime.CompilerServices;
using static CommonBytecode.BytecodeValueType;

namespace CommonBytecode;

public static class BytecodeValueExtensions
{
    public static string ToStringExtension(this BytecodeValue value) => ToStringValue(value.Value, value.Type);

    public static string ToStringValue(Any value, BytecodeValueType type)
    {
#if DEBUG
        return CatchExceptionsToStringValue(value, type);
#else
        return BasicToStringValue(value, type);
#endif
    }

    private static string CatchExceptionsToStringValue(Any value, BytecodeValueType type)
    {
        try
        {
            return BasicToStringValue(value, type);
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

    private static string BasicToStringValue(Any value, BytecodeValueType type)
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

    public static string UnsafeI64ToString(long value, BytecodeValueType type)
    {
#if DEBUG
        return CatchExceptionsUnsafeI64ToString(value, type);
#else
        return BasicUnsafeI64ToString(value, type);
#endif
    }

    private static string CatchExceptionsUnsafeI64ToString(long value, BytecodeValueType type)
    {
        try
        {
            return BasicUnsafeI64ToString(value, type);
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

    private static string BasicUnsafeI64ToString(long value, BytecodeValueType type)
    {
        return type switch
        {
            Nil => "Nil",
            Number => Unsafe.BitCast<long, double>(value).ToString(CultureInfo.InvariantCulture),
            SharpFunctionAddress => Unsafe.BitCast<long, nint>(value).ToString("X"),
            NativeI64 => $"n_{value}",
            BytecodeValueType.Any => $"any: {value}",
            _ => Throw.InvalidOpEx<string>(),
        };
    }
}