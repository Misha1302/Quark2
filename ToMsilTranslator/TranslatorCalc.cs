using Doubles;

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