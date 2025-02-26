namespace SharpAnyType;

/// <summary>
///     A structure for storing values of any type with their types specified.
/// </summary>
public readonly struct Any : IEquatable<Any>
{
    /// <summary>
    ///     The 'Nil' value (empty value).
    /// </summary>
    public static readonly Any Nil = new(null!, AnyValueType.Nil);

    /// <summary>
    ///     Holds the object representation of the value.
    /// </summary>
    public readonly object Value;

    /// <summary>
    ///     The type of the stored value.
    /// </summary>
    public readonly AnyValueType Type;

    /// <summary>
    ///     Constructor that takes in a value and its type.
    /// </summary>
    /// <param name="value">The object representation of the value.</param>
    /// <param name="type">The type of the value.</param>
    public Any(object value, AnyValueType type)
    {
        Value = value;
        Type = type;
    }

    /// <summary>
    ///     Overridden method to compare objects. Uses an equality comparer instance to check for equality.
    /// </summary>
    /// <param name="obj">The object to be compared against.</param>
    /// <returns>true if the objects are equal, otherwise false.</returns>
    public override bool Equals(object? obj) =>
        AnyEqualityComparer.Instance.Equals(this, obj as Any? ?? default);

    /// <summary>
    ///     Overridden method to get the hash code. Uses an equality comparer instance to compute the hash.
    /// </summary>
    /// <returns>The hash code of the current instance.</returns>
    public override int GetHashCode() => AnyEqualityComparer.Instance.GetHashCode(this);

    /// <summary>
    ///     Method to cast the stored value to type T.
    /// </summary>
    /// <typeparam name="T">The target type to which the value should be casted.</typeparam>
    /// <returns>The casted value.</returns>
    public T Get<T>() => (T)Value;

    /// <summary>
    ///     Implicit conversion operator from a double value to the Any structure.
    /// </summary>
    /// <param name="value">A double value.</param>
    public static implicit operator Any(double value) => new(value, Number);

    /// <summary>
    ///     Implicit conversion operator from a string to the Any structure.
    /// </summary>
    /// <param name="value">A string value.</param>
    public static implicit operator Any(string value) => new(value, Str);

    /// <summary>
    ///     Overridden ToString() method that returns a string representation of the value based on its type.
    /// </summary>
    /// <returns>A string representation of the value.</returns>
    public override string ToString() => this.AnyToString(Type);

    /// <summary>
    ///     Equality operator for two instances of the Any structure.
    /// </summary>
    /// <param name="a">The first instance.</param>
    /// <param name="b">The second instance.</param>
    /// <returns>true if both instances are equal, otherwise false.</returns>
    public static bool operator ==(Any a, Any b) => AnyEqualityComparer.Instance.Equals(a, b);

    /// <summary>
    ///     Inequality operator for two instances of the Any structure.
    /// </summary>
    /// <param name="a">The first instance.</param>
    /// <param name="b">The second instance.</param>
    /// <returns>true if the instances are not equal, otherwise false.</returns>
    public static bool operator !=(Any a, Any b) => !(a == b);

    /// <summary>
    ///     Checks whether the value is considered true.
    /// </summary>
    /// <returns>true if the value is not null, false, or 0.0, otherwise false.</returns>
    public bool IsTrue() => Value is not (null or false or 0.0);

    /// <summary>
    ///     Compares the current instance with another instance of the Any structure.
    /// </summary>
    /// <param name="other">Another instance to compare against.</param>
    /// <returns>true if the values and types match, otherwise false.</returns>
    public bool Equals(Any other) => Value.Equals(other.Value) && Type == other.Type;
}