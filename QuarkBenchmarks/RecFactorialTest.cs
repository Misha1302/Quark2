namespace QuarkBenchmarks;

public class RecFactorialTest() : QuarkTest(
    """
    import "../../../../../../../../Libraries"

    def Main() {
        return Fact(1000)
    }

    def Fact(i) {
        if i <= 1 { return i }
        return Fact(i - 1) * i
    }
    """
)
{
    [Benchmark(Baseline = true)]
    public double TranslatorTest() => MsilExecutor.RunModule().First().Get<double>();

    [Benchmark]
    public double InterpreterTest() => Interpreter.RunModule().First().Get<double>();
}