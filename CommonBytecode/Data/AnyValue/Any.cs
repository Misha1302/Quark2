namespace CommonBytecode.Data.AnyValue;

public class Any(object value) : IEqualityComparer<Any>
{
    public static readonly Any Nil = new(null!) { Type = BytecodeValueType.Nil };

    public BytecodeValueType Type = BytecodeValueType.Any;

    public object Value = value;

    public bool Equals(Any? x, Any? y) => x.EqualExt(y);

    public int GetHashCode(Any obj) => HashCode.Combine(obj.Type, obj.Value);

    public T Get<T>() => (T)Value;

    public static implicit operator Any(double value) => new(value) { Type = Number };
    public static implicit operator Any(string value) => new(value) { Type = Str };

    public override string ToString() => this.AnyToString(Type);
}