using CommonDataStructures;
using Doubles;

namespace ToMsilTranslator;

public static class TranslatorCalc
{
    public static AnyOpt Sum(AnyOpt a, AnyOpt b) =>
        new(a.Get<double>() + b.Get<double>(), Number);

    public static AnyOpt Sub(AnyOpt a, AnyOpt b) =>
        new(a.Get<double>() - b.Get<double>(), Number);

    public static AnyOpt Mul(AnyOpt a, AnyOpt b) =>
        new(a.Get<double>() * b.Get<double>(), Number);

    public static AnyOpt Div(AnyOpt a, AnyOpt b) =>
        new(a.Get<double>() / b.Get<double>(), Number);

    public static AnyOpt Pow(AnyOpt a, AnyOpt b) =>
        new(Math.Pow(a.Get<double>(), b.Get<double>()), Number);

    public static AnyOpt And(AnyOpt a, AnyOpt b) => new(
        a.Get<double>().ToLong() & b.Get<double>().ToLong(),
        Number);

    public static AnyOpt Or(AnyOpt a, AnyOpt b) => new(
        a.Get<double>().ToLong() | b.Get<double>().ToLong(),
        Number);

    public static AnyOpt Xor(AnyOpt a, AnyOpt b) => new(
        a.Get<double>().ToLong() ^ b.Get<double>().ToLong(),
        Number);

    public static AnyOpt Not(AnyOpt a) => new(a.IsTrue() ? 0.0 : 1.0, Number);

    public static AnyOpt Eq(AnyOpt a, AnyOpt b)
    {
        if ((a.Type & Number) != 0)
            return new AnyOpt(a.Get<double>().EqualWithAccuracy(b.Get<double>(), 1e-5) ? 1.0 : 0.0,
                Number);
        return new AnyOpt(a.GetRef<string>() == b.GetRef<string>() ? 1.0 : 0.0, Number);
    }

    public static AnyOpt NotEq(AnyOpt a, AnyOpt b) =>
        Not(Eq(a, b));

    public static AnyOpt Lt(AnyOpt a, AnyOpt b) =>
        new(a.Get<double>() < b.Get<double>() ? 1.0 : 0.0, Number);

    public static AnyOpt Gt(AnyOpt a, AnyOpt b) =>
        new(a.Get<double>() > b.Get<double>() ? 1.0 : 0.0, Number);

    public static AnyOpt GtOrEq(AnyOpt a, AnyOpt b) =>
        new(Gt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, Number);

    public static AnyOpt LtOrEq(AnyOpt a, AnyOpt b) =>
        new(Lt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, Number);

    public static AnyOpt Mod(AnyOpt a, AnyOpt b) =>
        new(a.Get<double>() % b.Get<double>(), Number);
}