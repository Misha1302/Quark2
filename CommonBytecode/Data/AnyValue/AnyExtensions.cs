using System.Globalization;
using System.Runtime.CompilerServices;
using ConfigurationManager;
using ExceptionsManager;

namespace CommonBytecode.Data.AnyValue;

public static class AnyExtensions
{
    public static Any ToAny(this IAny value) =>
        new(value.GetObjectValue(), value.GetAnyType());

    public static Any ToAny(this object value, BytecodeValueType type = BytecodeValueType.Any) =>
        new(value, type);

    public static string AnyToString(this Any value, BytecodeValueType type) =>
        Configuration.IsDebug
            ? CatchExceptionsAnyToString(value, type)
            : BasicAnyToString(value, type);

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
            SomeSharpObject => value.Value is not Delegate d
                ? value.Value.ToString() ?? string.Empty
                : d.Method.ToString()!,
            NativeI64 => $"n_{value.Get<long>()}",
            BytecodeValueType.Any => $"any: {value.Value}",
            _ => Throw.InvalidOpEx<string>(),
        };
    }

    public static string UnsafeI64ToString(this long value, BytecodeValueType type) =>
        Configuration.IsDebug
            ? CatchExceptionsUnsafeI64ToString(value, type)
            : BasicUnsafeI64ToString(value, type);

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
            NativeI64 => $"n_{value}",
            BytecodeValueType.Any => $"any: {value}",
            _ => Throw.InvalidOpEx<string>(),
        };
    }

    public static bool EqualExt(this Any x, Any y)
    {
        if (x.GetType() != y.GetType()) return false;

        if (x.Type == Nil && y.Type == Nil) return true;
        if (x.Type == Nil) return false;
        if (y.Type == Nil) return false;

        return x.Type == y.Type && x.Value.Equals(y.Value);
    }
}