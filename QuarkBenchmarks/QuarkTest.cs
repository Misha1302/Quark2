using AsgToBytecodeTranslator;
using CommonBytecode;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkCFrontend;
using VirtualMachine;

namespace QuarkBenchmarks;

[MemoryDiagnoser]
[RPlotExporter] [CsvMeasurementsExporter]
public abstract class QuarkTest(string code)
{
    private QuarkVirtualMachine _interpreter = null!;
    private TranslatorToMsil.TranslatorToMsil _msilExecutor = null!;

    [GlobalSetup]
    public void Setup()
    {
        var lexemes = new QuarkLexer(QuarkLexerDefaultConfiguration.CreateDefault()).Lexemize(code);
        var asg = new AsgBuilder<QuarkLexemeType>(QuarkAsgBuilderConfiguration.CreateDefault()).Build(lexemes);
        var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg);
        _msilExecutor = new TranslatorToMsil.TranslatorToMsil();
        _msilExecutor.Init(new ExecutorConfiguration(module));
        _interpreter = new QuarkVirtualMachine();
        _interpreter.Init(new ExecutorConfiguration(module));
    }

    [Benchmark(Baseline = true)]
    public double TranslatorTest() => _msilExecutor.RunModule().First().Get<double>();

    [Benchmark]
    public double InterpreterTest() => _interpreter.RunModule().First().Get<double>();
}