using System.Numerics;
using SharpAnyType;

namespace QuarkComplex;

public class QuarkComplex
{
    public static Any CreateComplex(Any a, Any b) =>
        new(new Complex(a.Get<double>(), b.Get<double>()), AnyValueType.SomeSharpObject);

    public static Any SumComplex(Any a, Any b) =>
        new(a.Get<Complex>() + b.Get<Complex>(), AnyValueType.SomeSharpObject);

    public static Any SubComplex(Any a, Any b) =>
        new(a.Get<Complex>() - b.Get<Complex>(), AnyValueType.SomeSharpObject);

    public static Any MulComplex(Any a, Any b) =>
        new(a.Get<Complex>() * b.Get<Complex>(), AnyValueType.SomeSharpObject);

    public static Any DivComplex(Any a, Any b) =>
        new(a.Get<Complex>() / b.Get<Complex>(), AnyValueType.SomeSharpObject);

    public static Any AbsComplex(Any a) =>
        new(Complex.Abs(a.Get<Complex>()), AnyValueType.SomeSharpObject);
}