using AbstractExecutor;
using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;
using QuarkCFrontend;
using VirtualMachine;

namespace Quark2;

public class ProgramRunner
{
    public void Run(RunType runType)
    {
        if (runType == RunType.Measuring)
        {
            // Translator faster than interpreter in approximately 9.5 times 
            new Measurer().Run(2);
        }
        else
        {
            var code2 = File.ReadAllText("Code/Main.lua");

            var quarkStatistics = new QuarkStatistics();
            var lexemes =
                quarkStatistics.Measure(() => new Lexer(LexerDefaultConfiguration.CreateDefault()).Lexemize(code2));
            var asg = quarkStatistics.Measure(() =>
                new AsgBuilder(AsgBuilderConfiguration.CreateDefault()).Build(lexemes));
            var module =
                quarkStatistics.Measure(() => new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg));
            var executor = CreateExecutor(runType);
            executor.PrepareToRun(module);
            var results = quarkStatistics.Measure(() => executor.RunModule());

            Console.WriteLine($"Results: {string.Join(", ", results)}");
            Console.WriteLine("Statistics:");
            Console.WriteLine(quarkStatistics.ToString());
        }
    }

    private static IExecutor CreateExecutor(RunType runType)
    {
        var executor = (IExecutor)(
            runType == RunType.MainCodeRunningUsingInterpreter
                ? new QuarkVirtualMachine(new ExecutorConfiguration())
                : new ToMsilTranslator.ToMsilTranslator()
        );
        return executor;
    }
}