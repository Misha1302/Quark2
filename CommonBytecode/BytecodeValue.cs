namespace CommonBytecode;

public class BytecodeValue(Any value, BytecodeValueType type)
{
    public BytecodeValueType Type { get; set; } = type;
    public Any Value { get; set; } = value;

    public T Get<T>() => Value.Get<T>();
}