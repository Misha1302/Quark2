using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DynamicStrongTypeValue;
using SharpAnyType;

BenchmarkRunner.Run<AnyValueTest>();


[MemoryDiagnoser]
[RPlotExporter] [CsvMeasurementsExporter]
public abstract class AnyValueTest
{
    private readonly AnyOpt _aDouble = AnyOpt.Create(2, AnyValueType.Number);
    private readonly AnyOpt _aShell = AnyOpt.CreateRef(new DoubleShell(2), AnyValueType.SomeSharpObject);
    private readonly AnyOpt _bDouble = AnyOpt.Create(3, AnyValueType.Number);
    private readonly AnyOpt _bShell = AnyOpt.CreateRef(new DoubleShell(3), AnyValueType.SomeSharpObject);

    [Benchmark(Baseline = true)]
    public AnyOpt DoubleTypeSum() => AnyOptCalculator.Sum(_aDouble, _bDouble);

    [Benchmark]
    public AnyOpt DynamicTypeSum() => AnyOptCalculator.Sum(_aShell, _bShell);

    public readonly struct DoubleShell(double value)
    {
        private readonly double _value = value;

        public static DoubleShell operator +(DoubleShell a, DoubleShell b) => new(a._value + b._value);
    }
}