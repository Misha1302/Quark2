namespace DynamicStrongTypeValue;

public readonly struct AnyOpt : IAny
{
    public readonly AnyValueType Type = Nil;

    private readonly long _value = 0;
    private readonly object _ref = null!;

    private AnyOpt(object @ref, AnyValueType type)
    {
        _ref = @ref;
        Type = type;
    }

    private AnyOpt(long value, AnyValueType type)
    {
        _value = value;
        Type = type;
    }

    public static readonly AnyOpt NilValue = new(0, Nil);

    /// <summary>
    ///     Create new instance of AnyOpt
    /// </summary>
    /// <param name="value">unmanaged 8-byte value</param>
    /// <param name="type">type of value</param>
    /// <typeparam name="T">unmanaged 8-byte type</typeparam>
    /// <returns>new AnyOpt instance</returns>
    public static AnyOpt Create<T>(T value, AnyValueType type) where T : struct =>
        new(Unsafe.BitCast<T, long>(value), type);

    public static AnyOpt Create(int value, AnyValueType type) => Create((long)value, type);

    /// <summary>
    ///     Create new instance of AnyOpt that's value saving into heap
    /// </summary>
    /// <param name="value">any value (will be boxed if it's not heap value)</param>
    /// <param name="type">type of value</param>
    /// <typeparam name="T">any type</typeparam>
    /// <returns>new AnyOpt instance</returns>
    public static AnyOpt CreateRef<T>(T value, AnyValueType type) => new(value!, type);

    /// <summary>
    ///     Unsafe get 8-byte unmanaged type value
    /// </summary>
    /// <typeparam name="T">8-byte unmanaged type</typeparam>
    /// <returns>T-type value</returns>
    public T Get<T>() where T : struct => Unsafe.BitCast<long, T>(_value);

    /// <summary>
    ///     Safe get value. Works correctly only if was created by 'CreateRef'
    /// </summary>
    /// <typeparam name="T">any type</typeparam>
    /// <returns>T-type value</returns>
    public T GetRef<T>() => (T)_ref;

    public override string ToString() =>
        Type.IsRefType()
            ? _ref.ObjectToAny().AnyToString(Type)
            : _value.UnsafeI64ToString(Type);

    /// <summary>
    ///     Returns is this value not empty
    /// </summary>
    /// <returns>true if the value is not empty</returns>
    public bool IsTrue() => _value != 0 || _ref != null;

    public object GetObjectValue() => this.GetValueInSharpType();

    public AnyValueType GetAnyType() => Type;
}