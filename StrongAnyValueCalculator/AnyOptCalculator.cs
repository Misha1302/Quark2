using System.Runtime.CompilerServices;
using DynamicStrongTypeValue;

namespace StrongAnyValueCalculator;

public static class AnyOptCalculator
{
    // Math ops
    public static AnyOpt Sum(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() + b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() + (dynamic)b.GetRef<object>(), a.Type);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Sub(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() - b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() - (dynamic)b.GetRef<object>(), a.Type);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Validate(in AnyOpt a, in AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, $"Types to perform operations must be equal ({a.Type}, {b.Type})");
    }

    public static AnyOpt Mul(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() * b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() * (dynamic)b.GetRef<object>(), a.Type);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Div(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() / b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() / (dynamic)b.GetRef<object>(), a.Type);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Pow(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(Math.Pow(a.Get<double>(), b.Get<double>()), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef(((dynamic)a.GetRef<object>()).Pow((dynamic)b.GetRef<object>()), Number);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Mod(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() % b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() % (dynamic)b.GetRef<object>(), a.Type);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }


    // Logic ops
    public static AnyOpt And(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>().ToLong() & b.Get<double>().ToLong(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() & (dynamic)b.GetRef<object>(), a.Type);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Or(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>().ToLong() | b.Get<double>().ToLong(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() | (dynamic)b.GetRef<object>(), a.Type);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Xor(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>().ToLong() ^ b.Get<double>().ToLong(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() ^ (dynamic)b.GetRef<object>(), a.Type);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Not(in AnyOpt a)
    {
        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.IsTrue() ? 0.0 : 1.0, Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef(!(dynamic)a.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }


    // Compare ops
    public static AnyOpt Eq(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>().EqualWithAccuracy(b.Get<double>(), 1e-5) ? 1.0 : 0.0, Number);
        if (a.Type.HasFlagFast(Str))
            return AnyOpt.Create(a.GetRef<string>() == b.GetRef<string>() ? 1.0 : 0.0, Number);
        if (a.Type.IsRefType())
            return AnyOpt.Create((dynamic)a.GetRef<object>() == (dynamic)b.GetRef<object>() ? 1.0 : 0.0, Number);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt NotEq(in AnyOpt a, in AnyOpt b) =>
        Not(Eq(a, b));

    public static AnyOpt Lt(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() < b.Get<double>() ? 1.0 : 0.0, Number);
        if (a.Type.IsRefType())
            return AnyOpt.Create((dynamic)a.GetRef<object>() < (dynamic)b.GetRef<object>() ? 1.0 : 0.0, Number);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Gt(in AnyOpt a, in AnyOpt b)
    {
        Validate(a, b);

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() > b.Get<double>() ? 1.0 : 0.0, Number);
        if (a.Type.IsRefType())
            return AnyOpt.Create((dynamic)a.GetRef<object>() > (dynamic)b.GetRef<object>() ? 1.0 : 0.0, Number);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt GtOrEq(in AnyOpt a, in AnyOpt b) =>
        AnyOpt.Create(Gt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, Number);

    public static AnyOpt LtOrEq(in AnyOpt a, in AnyOpt b) =>
        AnyOpt.Create(Lt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, Number);
}