namespace CommonBytecode.Data.AnyValue;

public class Any(object value)
{
    public static readonly Any Nil = new(null!) { Type = BytecodeValueType.Nil };

    public readonly object Value = value;

    public BytecodeValueType Type = BytecodeValueType.Any;

    public override bool Equals(object? obj) => AnyEqualityComparer.Instance.Equals(this, obj as Any);

    public override int GetHashCode() => AnyEqualityComparer.Instance.GetHashCode(this);

    public T Get<T>() => (T)Value;

    public static implicit operator Any(double value) => new(value) { Type = Number };
    public static implicit operator Any(string value) => new(value) { Type = Str };

    public override string ToString() => this.AnyToString(Type);

    public static bool operator ==(Any a, Any b) => AnyEqualityComparer.Instance.Equals(a, b);

    public static bool operator !=(Any a, Any b) => !(a == b);
}