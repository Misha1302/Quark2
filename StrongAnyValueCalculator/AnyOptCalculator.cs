namespace DynamicStrongTypeValue;

public static class AnyOptCalculator
{
    // Math ops
    public static AnyOpt Sum(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() + b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() + (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Sub(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() - b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() - (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Mul(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() * b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() * (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Div(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() / b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() / (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Pow(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() / b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef(((dynamic)a.GetRef<object>()).Pow((dynamic)b.GetRef<object>()), Number);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Mod(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() % b.Get<double>(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() % (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }


    // Logic ops
    public static AnyOpt And(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>().ToLong() & b.Get<double>().ToLong(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() & (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Or(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>().ToLong() | b.Get<double>().ToLong(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() | (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Xor(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>().ToLong() ^ b.Get<double>().ToLong(), Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() ^ (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Not(AnyOpt a)
    {
        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.IsTrue() ? 0.0 : 1.0, Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef(!(dynamic)a.GetRef<object>(), Number);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }


    // Compare ops
    public static AnyOpt Eq(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>().EqualWithAccuracy(b.Get<double>(), 1e-5) ? 1.0 : 0.0, Number);
        if (a.Type.HasFlagFast(Str))
            return AnyOpt.Create(a.GetRef<string>() == b.GetRef<string>() ? 1.0 : 0.0, Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() == (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt NotEq(AnyOpt a, AnyOpt b) =>
        Not(Eq(a, b));

    public static AnyOpt Lt(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() < b.Get<double>() ? 1.0 : 0.0, Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() < (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt Gt(AnyOpt a, AnyOpt b)
    {
        Throw.AssertAlways(a.Type == b.Type, "Types to perform operations must be equal");

        if (a.Type.HasFlagFast(Number))
            return AnyOpt.Create(a.Get<double>() > b.Get<double>() ? 1.0 : 0.0, Number);
        if (a.Type.IsRefType())
            return AnyOpt.CreateRef((dynamic)a.GetRef<object>() > (dynamic)b.GetRef<object>(), SomeSharpObject);

        return Throw.InvalidOpEx<AnyOpt>("Cannot perform operation");
    }

    public static AnyOpt GtOrEq(AnyOpt a, AnyOpt b) =>
        AnyOpt.Create(Gt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, Number);

    public static AnyOpt LtOrEq(AnyOpt a, AnyOpt b) =>
        AnyOpt.Create(Lt(a, b).IsTrue() || Eq(a, b).IsTrue() ? 1.0 : 0.0, Number);
}