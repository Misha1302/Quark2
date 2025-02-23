using BenchmarkDotNet.Attributes;

namespace QuarkBenchmarks;

public class MillionLoopTest() : QuarkTest(
    """
    import "../../../../../../../../Libraries"

    def Main() {
        sum = 0
        for (i = 0) (i < 1000000) (i = i + 1) {
            sum = sum + i
        }
        return sum
    }
    """
)
{
    [Benchmark(Baseline = true)]
    public double TranslatorTest() => MsilExecutor.RunModule().First().Get<double>();

    [Benchmark]
    public double InterpreterTest() => Interpreter.RunModule().First().Get<double>();
}