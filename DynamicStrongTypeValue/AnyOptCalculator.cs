namespace DynamicStrongTypeValue;

public static class AnyOptCalculator
{
    public static AnyOpt Sum(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(a.Get<double>() + b.Get<double>(), Number);
    }

    public static AnyOpt Sub(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(a.Get<double>() - b.Get<double>(), Number);
    }

    public static AnyOpt Mul(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(a.Get<double>() * b.Get<double>(), Number);
    }

    public static AnyOpt Div(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(a.Get<double>() / b.Get<double>(), Number);
    }

    public static AnyOpt Pow(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(Math.Pow(a.Get<double>(), b.Get<double>()), Number);
    }

    public static AnyOpt And(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(
            a.Get<double>().ToLong() & b.Get<double>().ToLong(),
            Number);
    }

    public static AnyOpt Or(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(
            a.Get<double>().ToLong() | b.Get<double>().ToLong(),
            Number);
    }

    public static AnyOpt Xor(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(
            a.Get<double>().ToLong() ^ b.Get<double>().ToLong(),
            Number);
    }

    public static AnyOpt Not(AnyOpt a)
    {
        Throw.AssertAlways(a.Type.HasFlagFast(Number));
        return new AnyOpt(a.IsTrue() ? 0.0 : 1.0, Number);
    }

    public static AnyOpt Eq(AnyOpt a, AnyOpt b)
    {
        if (a.Type == b.Type && a.Type.HasFlagFast(Str))
            return new AnyOpt(a.GetRef<string>() == b.GetRef<string>() ? 1.0 : 0.0, Number);

        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(a.Get<double>().EqualWithAccuracy(b.Get<double>(), 1e-5) ? 1.0 : 0.0, Number);
    }

    public static AnyOpt NotEq(AnyOpt a, AnyOpt b) =>
        Not(Eq(a, b));

    public static AnyOpt Lt(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(a.Get<double>() < b.Get<double>() ? 1.0 : 0.0, Number);
    }

    public static AnyOpt Gt(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(a.Get<double>() > b.Get<double>() ? 1.0 : 0.0, Number);
    }

    public static AnyOpt GtOrEq(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(Gt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, Number);
    }

    public static AnyOpt LtOrEq(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(Lt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, Number);
    }

    public static AnyOpt Mod(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type && a.Type.HasFlagFast(Number));
        return new AnyOpt(a.Get<double>() % b.Get<double>(), Number);
    }
}