namespace CommonBytecode;

public static class AnyExtensions
{
    public static Any ToAny(this IAny value) => new(value.GetObjectValue()) { Type = value.GetAnyType() };
    public static Any ToAny(this object value) => new(value);
}