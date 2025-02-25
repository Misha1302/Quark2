using AbstractExecutor;
using AsgToBytecodeTranslator;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkCFrontend;
using VirtualMachine;

namespace QuarkBenchmarks;

[MemoryDiagnoser]
[RPlotExporter] [CsvMeasurementsExporter]
public abstract class QuarkTest(string code)
{
    protected QuarkVirtualMachine Interpreter = null!;
    protected ToMsilTranslator.ToMsilTranslator MsilExecutor = null!;

    [GlobalSetup]
    public void Setup()
    {
        var lexemes = new Lexer(QuarkLexerDefaultConfiguration.CreateDefault()).Lexemize(code);
        var asg = new AsgBuilder<QuarkLexemeType>(QuarkAsgBuilderConfiguration.CreateDefault()).Build(lexemes);
        var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg);
        MsilExecutor = new ToMsilTranslator.ToMsilTranslator();
        MsilExecutor.Init(new ExecutorConfiguration(module));
        Interpreter = new QuarkVirtualMachine();
        Interpreter.Init(new ExecutorConfiguration(module));
    }
}