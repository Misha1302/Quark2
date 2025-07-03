namespace GenericBytecode.Structures;

public readonly record struct Boolean(bool Value) : IBoolean
{
    public bool ToBool() => Value;

    public static implicit operator bool(Boolean value) => value.ToBool();
}