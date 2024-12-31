using CommonBytecode.Data.AnyValue;
using Doubles;

namespace ToMsilTranslator;

public static class TranslatorCalc
{
    public static Any Sum(Any a, Any b) => new(a.Get<double>() + b.Get<double>(), BytecodeValueType.Number);

    public static Any Sub(Any a, Any b) => new(a.Get<double>() - b.Get<double>(), BytecodeValueType.Number);

    public static Any Mul(Any a, Any b) => new(a.Get<double>() * b.Get<double>(), BytecodeValueType.Number);

    public static Any Div(Any a, Any b) => new(a.Get<double>() / b.Get<double>(), BytecodeValueType.Number);

    public static Any Pow(Any a, Any b) => new(Math.Pow(a.Get<double>(), b.Get<double>()), BytecodeValueType.Number);

    public static Any And(Any a, Any b) => new((double)(a.Get<double>().ToLong() & b.Get<double>().ToLong()),
        BytecodeValueType.Number);

    public static Any Or(Any a, Any b) => new((double)(a.Get<double>().ToLong() | b.Get<double>().ToLong()),
        BytecodeValueType.Number);

    public static Any Xor(Any a, Any b) => new((double)(a.Get<double>().ToLong() ^ b.Get<double>().ToLong()),
        BytecodeValueType.Number);

    public static Any Not(Any a) => new(a.IsTrue() ? 0.0 : 1.0, BytecodeValueType.Number);

    public static Any Eq(Any a, Any b)
    {
        if ((a.Type & BytecodeValueType.Number) != 0)
            return new Any(a.Get<double>().EqualWithAccuracy(b.Get<double>(), 1e-5) ? 1.0 : 0.0,
                BytecodeValueType.Number);
        return new Any(a.Get<string>() == b.Get<string>() ? 1.0 : 0.0, BytecodeValueType.Number);
    }

    public static Any NotEq(Any a, Any b) =>
        Not(Eq(a, b));

    public static Any Lt(Any a, Any b) => new(a.Get<double>() < b.Get<double>() ? 1.0 : 0.0, BytecodeValueType.Number);

    public static Any Gt(Any a, Any b) => new(a.Get<double>() > b.Get<double>() ? 1.0 : 0.0, BytecodeValueType.Number);

    public static Any GtOrEq(Any a, Any b) =>
        new(Gt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, BytecodeValueType.Number);

    public static Any LtOrEq(Any a, Any b) =>
        new(Lt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, BytecodeValueType.Number);

    public static Any Mod(Any a, Any b) => new(a.Get<double>() % b.Get<double>(), BytecodeValueType.Number);
}