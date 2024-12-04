using CommonBytecode.Interfaces;

namespace VirtualMachine.Vm.Data;

public readonly struct VmValue : IAny
{
    public readonly BytecodeValueType Type = Nil;

    private readonly long _value = 0;
    private readonly object _ref = null!;

    private VmValue(object @ref, BytecodeValueType type)
    {
        _ref = @ref;
        Type = type;
    }

    private VmValue(long value, BytecodeValueType type)
    {
        _value = value;
        Type = type;
    }

    public static readonly VmValue NilValue = new(0, Nil);

    /// <summary>
    ///     Create new instance of VmValue
    /// </summary>
    /// <param name="value">Any 8-byte value</param>
    /// <param name="type">VmValueType value describing type of value</param>
    /// <typeparam name="T">Any unmanaged 8-byte type</typeparam>
    /// <returns>new VmValue instance</returns>
    public static VmValue Create<T>(T value, BytecodeValueType type) where T : unmanaged
    {
        if (typeof(T) == typeof(int))
            return new VmValue((int)(object)value, type);

        if (typeof(T).IsEnum)
            Throw.Assert(Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T))) == 8);

        else if (Marshal.SizeOf<T>() != 8)
            Throw.InvalidOpEx();

        return new VmValue(Unsafe.BitCast<T, long>(value), type);
    }

    /// <summary>
    ///     Create new instance of VmValue
    /// </summary>
    /// <param name="value">Any reference type</param>
    /// <param name="type">VmValueType value describing type of value</param>
    /// <typeparam name="T">Any reference type of value</typeparam>
    /// <returns>New Vmvalue instance</returns>
    public static VmValue CreateRef<T>(T value, BytecodeValueType type) where T : class => new(value, type);

    /// <summary>
    ///     Unsafe get any 8-byte unmanaged type value
    /// </summary>
    /// <typeparam name="T">8-byte unmanaged type</typeparam>
    /// <returns>T-type value</returns>
    public T Get<T>() where T : unmanaged => Unsafe.BitCast<long, T>(_value);

    /// <summary>
    ///     Safe get any reference type value
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