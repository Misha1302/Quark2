namespace CommonBytecode;

public class Any(object value)
{
    public BytecodeValueType Type = BytecodeValueType.Any;

    public object Value = value;

    public T Get<T>() => (T)Value;

    public static implicit operator Any(double value) => new(value);
    public static implicit operator Any(MathLogicOp value) => new(value);
    public static implicit operator Any(string value) => new(value);

    public override string ToString() => BytecodeValueExtensions.ToStringValue(this, Type);
}