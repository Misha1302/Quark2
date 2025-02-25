using Doubles;

namespace VirtualMachine.Vm.Execution;

public static class VmCalc
{
    public static AnyOpt Sum(AnyOpt a, AnyOpt b) => AnyOpt.Create(a.Get<double>() + b.Get<double>(), Number);

    public static AnyOpt Sub(AnyOpt a, AnyOpt b) => AnyOpt.Create(a.Get<double>() - b.Get<double>(), Number);

    public static AnyOpt Mul(AnyOpt a, AnyOpt b) => AnyOpt.Create(a.Get<double>() * b.Get<double>(), Number);

    public static AnyOpt Div(AnyOpt a, AnyOpt b) => AnyOpt.Create(a.Get<double>() / b.Get<double>(), Number);

    public static AnyOpt Pow(AnyOpt a, AnyOpt b) =>
        AnyOpt.Create(Math.Pow(a.Get<double>(), b.Get<double>()), Number);

    public static AnyOpt And(AnyOpt a, AnyOpt b) =>
        AnyOpt.Create((double)(a.Get<double>().ToLong() & b.Get<double>().ToLong()), Number);

    public static AnyOpt Or(AnyOpt a, AnyOpt b) =>
        AnyOpt.Create((double)(a.Get<double>().ToLong() | b.Get<double>().ToLong()), Number);

    public static AnyOpt Xor(AnyOpt a, AnyOpt b) =>
        AnyOpt.Create((double)(a.Get<double>().ToLong() ^ b.Get<double>().ToLong()), Number);

    public static AnyOpt Not(AnyOpt a) => AnyOpt.Create(a.IsTrue() ? 0.0 : 1.0, Number);

    public static AnyOpt Eq(AnyOpt a, AnyOpt b, double accuracy)
    {
        if ((a.Type & Number) != 0)
            return AnyOpt.Create(a.Get<double>().EqualWithAccuracy(b.Get<double>(), accuracy) ? 1.0 : 0.0, Number);
        return AnyOpt.Create(a.GetRef<string>() == b.GetRef<string>() ? 1.0 : 0.0, Number);
    }

    public static AnyOpt NotEq(AnyOpt a, AnyOpt b, double accuracy) =>
        Not(Eq(a, b, accuracy));

    public static AnyOpt Lt(AnyOpt a, AnyOpt b) =>
        AnyOpt.Create(a.Get<double>() < b.Get<double>() ? 1.0 : 0.0, Number);

    public static AnyOpt Gt(AnyOpt a, AnyOpt b) =>
        AnyOpt.Create(a.Get<double>() > b.Get<double>() ? 1.0 : 0.0, Number);

    public static AnyOpt GtOrEq(AnyOpt a, AnyOpt b, double accuracy) =>
        AnyOpt.Create(Gt(a, b).IsTrue() || Eq(a, b, accuracy).IsTrue() ? 1.0 : 0.0, Number);

    public static AnyOpt LtOrEq(AnyOpt a, AnyOpt b, double accuracy) =>
        AnyOpt.Create(Lt(a, b).IsTrue() || Eq(a, b, accuracy).IsTrue() ? 1.0 : 0.0, Number);

    public static AnyOpt Mod(AnyOpt a, AnyOpt b) => AnyOpt.Create(a.Get<double>() % b.Get<double>(), Number);
}