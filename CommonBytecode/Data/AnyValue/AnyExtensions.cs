using System.Globalization;
using System.Runtime.CompilerServices;
using ExceptionsManager;

namespace CommonBytecode.Data.AnyValue;

public static class AnyExtensions
{
    public static Any ToAny(this IAny value) => new(value.GetObjectValue()) { Type = value.GetAnyType() };
    public static Any ToAny(this object value) => new(value);

    public static string AnyToString(this Any value, BytecodeValueType type)
    {
#if DEBUG
        return CatchExceptionsAnyToString(value, type);
#else
        return BasicAnyToString(value, type);
#endif
    }

    private static string CatchExceptionsAnyToString(Any value, BytecodeValueType type)
    {
        try
        {
            return BasicAnyToString(value, type);
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }

    private static string BasicAnyToString(Any value, BytecodeValueType type)
    {
        return type switch
        {
            Nil => "Nil",
            Number => value.Get<double>().ToString(CultureInfo.InvariantCulture),
            Str => value.Get<string>(),
            SomeSharpObject => value.Value.ToString() ?? string.Empty,
            SharpFunctionAddress => value.Get<nint>().ToString("X"),
            NativeI64 => $"n_{value.Get<long>()}",
            BytecodeValueType.Any => $"any: {value.Get<long>()}",
            _ => Throw.InvalidOpEx<string>(),
        };
    }

    public static string UnsafeI64ToString(this long value, BytecodeValueType type)
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