using System.Numerics;
using DynamicStrongTypeValue;
using StrongAnyValueCalculator;

namespace UnitTests;

public class AnyOptTests
{
    [Test]
    public void TestA8()
    {
        var a = new Complex(2, 3);
        var b = new Complex(3, 4);
        var c = a + b;

        var anyOptA = AnyOpt.CreateRef(a, AnyValueType.SomeSharpObject);
        var anyOptB = AnyOpt.CreateRef(b, AnyValueType.SomeSharpObject);

        Assert.That(AnyOptCalculator.Sum(anyOptA, anyOptB).GetRef<Complex>(), Is.EqualTo(c));
    }

    [Test]
    public void TestA9()
    {
        Assert.Multiple(() =>
        {
            Assert.That(AnyOpt.NilValue.ToString(), Is.EqualTo("Nil"));
            Assert.That(AnyOpt.Create(-1.23, AnyValueType.Number).ToString(), Is.EqualTo("-1.23"));
            Assert.That(AnyOpt.CreateRef("Hi", AnyValueType.Str).ToString(), Is.EqualTo("Hi"));
            Assert.That(AnyOpt.Create(3, AnyValueType.NativeI64).ToString(), Is.EqualTo("n_3"));
            Assert.That(AnyOpt.CreateRef<object>(null!, AnyValueType.Any).ToString(), Is.EqualTo("any: "));
        });
    }

    [Test]
    public void TestB1()
    {
        Assert.Multiple(() =>
        {
            Assert.That(AnyOpt.Create(0.0, AnyValueType.Number).IsTrue(), Is.EqualTo(false));
            Assert.That(AnyOpt.Create(1.0, AnyValueType.Number).IsTrue(), Is.EqualTo(true));
            Assert.That(AnyOpt.CreateRef(123.0, AnyValueType.SomeSharpObject).IsTrue(), Is.EqualTo(true));
            Assert.That(AnyOpt.CreateRef<object>(null!, AnyValueType.SomeSharpObject).IsTrue(), Is.EqualTo(false));
        });
    }
}