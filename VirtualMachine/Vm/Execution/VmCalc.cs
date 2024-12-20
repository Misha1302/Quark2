using Doubles;

namespace VirtualMachine.Vm.Execution;

public static class VmCalc
{
    public static VmValue Sum(VmValue a, VmValue b) => VmValue.Create(a.Get<double>() + b.Get<double>(), Number);

    public static VmValue Sub(VmValue a, VmValue b) => VmValue.Create(a.Get<double>() - b.Get<double>(), Number);

    public static VmValue Mul(VmValue a, VmValue b) => VmValue.Create(a.Get<double>() * b.Get<double>(), Number);

    public static VmValue Div(VmValue a, VmValue b) => VmValue.Create(a.Get<double>() / b.Get<double>(), Number);

    public static VmValue Pow(VmValue a, VmValue b) =>
        VmValue.Create(Math.Pow(a.Get<double>(), b.Get<double>()), Number);

    public static VmValue And(VmValue a, VmValue b) =>
        VmValue.Create((double)(a.Get<double>().ToLong() & b.Get<double>().ToLong()), Number);

    public static VmValue Or(VmValue a, VmValue b) =>
        VmValue.Create((double)(a.Get<double>().ToLong() | b.Get<double>().ToLong()), Number);

    public static VmValue Xor(VmValue a, VmValue b) =>
        VmValue.Create((double)(a.Get<double>().ToLong() ^ b.Get<double>().ToLong()), Number);

    public static VmValue Not(VmValue a) => VmValue.Create(a.IsTrue() ? 0.0 : 1.0, Number);

    public static VmValue Eq(VmValue a, VmValue b, double accuracy) =>
        VmValue.Create(a.Get<double>().EqualWithAccuracy(b.Get<double>(), accuracy) ? 1.0 : 0.0, Number);

    public static VmValue NotEq(VmValue a, VmValue b, double accuracy) =>
        VmValue.Create(a.Get<double>().EqualWithAccuracy(b.Get<double>(), accuracy) ? 0.0 : 1.0, Number);

    public static VmValue Lt(VmValue a, VmValue b) =>
        VmValue.Create(a.Get<double>() < b.Get<double>() ? 1.0 : 0.0, Number);

    public static VmValue Gt(VmValue a, VmValue b) =>
        VmValue.Create(a.Get<double>() > b.Get<double>() ? 1.0 : 0.0, Number);

    public static VmValue GtOrEq(VmValue a, VmValue b, double accuracy) =>
        VmValue.Create(Gt(a, b).IsTrue() || Eq(a, b, accuracy).IsTrue() ? 1.0 : 0.0, Number);

    public static VmValue LtOrEq(VmValue a, VmValue b, double accuracy) =>
        VmValue.Create(Lt(a, b).IsTrue() || Eq(a, b, accuracy).IsTrue() ? 1.0 : 0.0, Number);

    public static VmValue Mod(VmValue a, VmValue b) => VmValue.Create(a.Get<double>() % b.Get<double>(), Number);
}