namespace GenericBytecode.Interfaces.DefaultImplementations;

public readonly record struct Boolean(bool Value) : IBoolean, IBasicValue
{
    public bool ToBool() => Value;

    public static implicit operator bool(Boolean value) => value.ToBool();
}