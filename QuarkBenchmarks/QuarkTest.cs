using AbstractExecutor;
using BenchmarkDotNet.Attributes;
using CommonBytecode.Data.Structures;
using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;
using QuarkCFrontend;
using VirtualMachine;

namespace QuarkBenchmarks;

[MemoryDiagnoser]
[RPlotExporter] [CsvMeasurementsExporter]
public abstract class QuarkTest(string code)
{
    protected QuarkVirtualMachine Interpreter = null!;
    protected BytecodeModule Module = null!;
    protected ToMsilTranslator.ToMsilTranslator MsilExecutor = null!;

    [GlobalSetup]
    public void Setup()
    {
        var lexemes = new Lexer(LexerDefaultConfiguration.CreateDefault()).Lexemize(code);
        var asg = new AsgBuilder(AsgBuilderConfiguration.CreateDefault()).Build(lexemes);
        Module = new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg);
        MsilExecutor = new ToMsilTranslator.ToMsilTranslator();
        Interpreter = new QuarkVirtualMachine(new ExecutorConfiguration());
    }
}