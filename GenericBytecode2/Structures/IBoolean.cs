namespace GenericBytecode2.Structures;

public interface IBoolean : IBasicValue
{
    public bool ToBool();
}

public readonly record struct Boolean(bool Value) : IBoolean
{
    public bool ToBool() => Value;

    public static implicit operator bool(Boolean value) => value.ToBool();
}