using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommonBytecode.Data.AnyValue;
using CommonBytecode.Interfaces;
using Doubles;
using ExceptionsManager;
using static CommonBytecode.Data.AnyValue.BytecodeValueType;

namespace ToMsilTranslator;

public static class TranslatorCalc
{
    public static TranslatorValue Sum(TranslatorValue a, TranslatorValue b) =>
        new(a.Get<double>() + b.Get<double>(), Number);

    public static TranslatorValue Sub(TranslatorValue a, TranslatorValue b) =>
        new(a.Get<double>() - b.Get<double>(), Number);

    public static TranslatorValue Mul(TranslatorValue a, TranslatorValue b) =>
        new(a.Get<double>() * b.Get<double>(), Number);

    public static TranslatorValue Div(TranslatorValue a, TranslatorValue b) =>
        new(a.Get<double>() / b.Get<double>(), Number);

    public static TranslatorValue Pow(TranslatorValue a, TranslatorValue b) =>
        new(Math.Pow(a.Get<double>(), b.Get<double>()), Number);

    public static TranslatorValue And(TranslatorValue a, TranslatorValue b) => new(
        a.Get<double>().ToLong() & b.Get<double>().ToLong(),
        Number);

    public static TranslatorValue Or(TranslatorValue a, TranslatorValue b) => new(
        a.Get<double>().ToLong() | b.Get<double>().ToLong(),
        Number);

    public static TranslatorValue Xor(TranslatorValue a, TranslatorValue b) => new(
        a.Get<double>().ToLong() ^ b.Get<double>().ToLong(),
        Number);

    public static TranslatorValue Not(TranslatorValue a) => new(a.IsTrue() ? 0.0 : 1.0, Number);

    public static TranslatorValue Eq(TranslatorValue a, TranslatorValue b)
    {
        if ((a.Type & Number) != 0)
            return new TranslatorValue(a.Get<double>().EqualWithAccuracy(b.Get<double>(), 1e-5) ? 1.0 : 0.0,
                Number);
        return new TranslatorValue(a.GetRef<string>() == b.GetRef<string>() ? 1.0 : 0.0, Number);
    }

    public static TranslatorValue NotEq(TranslatorValue a, TranslatorValue b) =>
        Not(Eq(a, b));

    public static TranslatorValue Lt(TranslatorValue a, TranslatorValue b) =>
        new(a.Get<double>() < b.Get<double>() ? 1.0 : 0.0, Number);

    public static TranslatorValue Gt(TranslatorValue a, TranslatorValue b) =>
        new(a.Get<double>() > b.Get<double>() ? 1.0 : 0.0, Number);

    public static TranslatorValue GtOrEq(TranslatorValue a, TranslatorValue b) =>
        new(Gt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, Number);

    public static TranslatorValue LtOrEq(TranslatorValue a, TranslatorValue b) =>
        new(Lt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, Number);

    public static TranslatorValue Mod(TranslatorValue a, TranslatorValue b) =>
        new(a.Get<double>() % b.Get<double>(), Number);
}

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

public static class TranslatorValueExtensions
{
    public static object GetValueInSharpType(this TranslatorValue value)
    {
        return value.Type switch
        {
            Nil => "Nil",
            Number => value.Get<double>(),
            Str => value.GetRef<string>(),
            SomeSharpObject => value.GetRef<object>(),
            NativeI64 => value.Get<long>(),
            BytecodeValueType.Any => value.Get<long>(),
            _ => Throw.InvalidOpEx<string>(),
        };
    }

    public static List<TranslatorValue> MakeTranslationValue(this Any any)
    {
        return any.Value switch
        {
            double d => [TranslatorValue.Create(d, Number)],
            long l => [TranslatorValue.Create(l, NativeI64)],
            bool l => [TranslatorValue.Create(l ? 1L : 0L, NativeI64)],
            string s => [TranslatorValue.CreateRef(s, Str)],
            Enum e => [TranslatorValue.Create((long)Convert.ToInt32(e), NativeI64)],
            null => [TranslatorValue.NilValue],
            var obj => [TranslatorValue.CreateRef(obj, SomeSharpObject)],
        };
    }
}