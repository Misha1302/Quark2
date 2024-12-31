using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ToMsilTranslator;

public readonly struct TranslatorValue : IAny
{
    public readonly BytecodeValueType Type = Nil;

    private readonly long _value = 0;
    private readonly object _ref = null!;

    private TranslatorValue(object @ref, BytecodeValueType type)
    {
        _ref = @ref;
        Type = type;
    }

    private TranslatorValue(long value, BytecodeValueType type)
    {
        _value = value;
        Type = type;
    }

    public TranslatorValue(double value, BytecodeValueType type)
    {
        _value = Unsafe.As<double, long>(ref value);
        Type = type;
    }

    public static readonly TranslatorValue NilValue = new(0, Nil);

    /// <summary>
    ///     Create new instance of VmValue
    /// </summary>
    /// <param name="value">TranslatorValue 8-byte value</param>
    /// <param name="type">VmValueType value describing type of value</param>
    /// <typeparam name="T">TranslatorValue unmanaged 8-byte type</typeparam>
    /// <returns>new VmValue instance</returns>
    public static TranslatorValue Create<T>(T value, BytecodeValueType type) where T : unmanaged
    {
        if (typeof(T) == typeof(int))
            return new TranslatorValue((int)(object)value, type);

        if (typeof(T).IsEnum)
            Throw.Assert(Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T))) == 8);

        else if (Marshal.SizeOf<T>() != 8)
            Throw.InvalidOpEx();

        return new TranslatorValue(Unsafe.BitCast<T, long>(value), type);
    }

    /// <summary>
    ///     Create new instance of VmValue
    /// </summary>
    /// <param name="value">TranslatorValue reference type</param>
    /// <param name="type">VmValueType value describing type of value</param>
    /// <typeparam name="T">TranslatorValue reference type of value</typeparam>
    /// <returns>New Vmvalue instance</returns>
    public static TranslatorValue CreateRef<T>(T value, BytecodeValueType type) where T : class => new(value, type);

    /// <summary>
    ///     Unsafe get TranslatorValue 8-byte unmanaged type value
    /// </summary>
    /// <typeparam name="T">8-byte unmanaged type</typeparam>
    /// <returns>T-type value</returns>
    public T Get<T>() where T : unmanaged => Unsafe.BitCast<long, T>(_value);

    /// <summary>
    ///     Safe get TranslatorValue reference type value
    /// </summary>
    /// <typeparam name="T">reference type</typeparam>
    /// <returns>T-type value</returns>
    public T GetRef<T>() where T : class => (T)_ref;

    public override string ToString() =>
        Type.IsRefType()
            ? _ref.ToAny().AnyToString(Type)
            : _value.UnsafeI64ToString(Type);

    /// <summary>
    ///     Returns is this value not empty
    /// </summary>
    /// <returns>true if the value is not empty</returns>
    public bool IsTrue() => _value != 0 || _ref != null;

    /// <summary>
    ///     Returns is this value empty
    /// </summary>
    /// <returns>true if the value is empty</returns>
    public bool IsFalse() => _value == 0;

    public object GetObjectValue() => this.GetValueInSharpType();
    public BytecodeValueType GetAnyType() => Type;
}