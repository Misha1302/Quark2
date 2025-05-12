namespace SharpAnyType;

public static class AnyExtensions
{
    public static Any ToAny(this IAny value) =>
        new(value.GetObjectValue(), value.GetAnyType());

    public static Any ObjectToAny(this object value, AnyValueType type = AnyValueType.Any) =>
        new(value, type);

    public static string AnyToString(this Any value, AnyValueType type) =>
        BasicAnyToString(value, type);

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
            _ => $"type: {type}",
        };
    }

    public static string UnsafeI64ToString(this long value, AnyValueType type) =>
        BasicUnsafeI64ToString(value, type);

    private static string BasicUnsafeI64ToString(long value, AnyValueType type)
    {
        return type switch
        {
            Nil => "Nil",
            Number => Unsafe.BitCast<long, double>(value).ToString(CultureInfo.InvariantCulture),
            NativeI64 => $"n_{value}",
            AnyValueType.Any => $"any: {value}",
            _ => $"type: {type}",
        };
    }
}