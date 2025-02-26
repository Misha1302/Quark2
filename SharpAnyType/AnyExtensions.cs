using System.Globalization;
using ConfigurationManager;

namespace SharpAnyType;

public static class AnyExtensions
{
    public static Any ToAny(this IAny value) =>
        new(value.GetObjectValue(), value.GetAnyType());

    public static Any ObjectToAny(this object value, AnyValueType type = AnyValueType.Any) =>
        new(value, type);

    public static string AnyToString(this Any value, AnyValueType type) =>
        Configuration.IsDebug
            ? CatchExceptionsAnyToString(value, type)
            : BasicAnyToString(value, type);

    private static string CatchExceptionsAnyToString(Any value, AnyValueType type)
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

    private static string BasicAnyToString(Any value, AnyValueType type)
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
            AnyValueType.Any => $"any: {value.Value}",
            _ => Throw.InvalidOpEx<string>(),
        };
    }

    public static string UnsafeI64ToString(this long value, AnyValueType type) =>
        Configuration.IsDebug
            ? CatchExceptionsUnsafeI64ToString(value, type)
            : BasicUnsafeI64ToString(value, type);

    private static string CatchExceptionsUnsafeI64ToString(long value, AnyValueType type)
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

    private static string BasicUnsafeI64ToString(long value, AnyValueType type)
    {
        return type switch
        {
            Nil => "Nil",
            Number => Unsafe.BitCast<long, double>(value).ToString(CultureInfo.InvariantCulture),
            NativeI64 => $"n_{value}",
            AnyValueType.Any => $"any: {value}",
            _ => Throw.InvalidOpEx<string>(),
        };
    }
}