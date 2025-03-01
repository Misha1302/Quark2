namespace TypesystemBenchmarks;

public class AnyValueAndShellTest
{
    private readonly AnyOpt _aDouble = AnyOpt.Create(2.0, AnyValueType.Number);
    private readonly AnyOpt _aShell = AnyOpt.CreateRef(2.0, AnyValueType.SomeSharpObject);
    private readonly AnyOpt _bDouble = AnyOpt.Create(3.0, AnyValueType.Number);
    private readonly AnyOpt _bShell = AnyOpt.CreateRef(3.0, AnyValueType.SomeSharpObject);

    [Benchmark(Baseline = true)]
    public AnyOpt DoubleTypeSum() => AnyOptCalculator.Sum(_aDouble, _bDouble);

    [Benchmark]
    public AnyOpt DynamicTypeSum() => AnyOptCalculator.Sum(_aShell, _bShell);
}