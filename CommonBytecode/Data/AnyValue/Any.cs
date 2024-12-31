using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ExceptionsManager;

namespace CommonBytecode.Data.AnyValue;

public readonly struct Any(object value, BytecodeValueType type = BytecodeValueType.Any) : IEquatable<Any>
{
    public static readonly Any Nil = new(null!, BytecodeValueType.Nil);

    public readonly object Value = value;

    public readonly BytecodeValueType Type = type;

    public override bool Equals(object? obj) =>
        AnyEqualityComparer.Instance.Equals(this, obj as Any? ?? default);

    public override int GetHashCode() => AnyEqualityComparer.Instance.GetHashCode(this);

    public T Get<T>() => (T)Value;

    public static implicit operator Any(double value) => new(value, Number);
    public static implicit operator Any(string value) => new(value, Str);

    public override string ToString() => this.AnyToString(Type);

    public static bool operator ==(Any a, Any b) => AnyEqualityComparer.Instance.Equals(a, b);

    public static bool operator !=(Any a, Any b) => !(a == b);

    public bool IsTrue() => Value is not (null or false or 0.0);

    public bool Equals(Any other) => Value.Equals(other.Value) && Type == other.Type;
}
