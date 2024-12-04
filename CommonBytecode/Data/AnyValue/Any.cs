namespace CommonBytecode.Data.AnyValue;

public class Any(object value)
{
    public BytecodeValueType Type = BytecodeValueType.Any;

    public object Value = value;

    public T Get<T>() => (T)Value;

    public static implicit operator Any(double value) => new(value) { Type = Number };
    public static implicit operator Any(string value) => new(value) { Type = Str };

    public override string ToString() => this.AnyToString(Type);
}