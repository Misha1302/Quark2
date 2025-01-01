using AbstractExecutor;
using QuarkCFrontend;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;
using VirtualMachine;

namespace Quark2;

public class Measurer
{
    public void Run(int repeatTimes)
    {
        var code = File.ReadAllText("Code/Main.lua");

        // something about 1655
        var min1 = Measure(
            () => new QuarkVirtualMachine(new ExecutorConfiguration()),
            repeatTimes, code
        );
        Console.WriteLine($"Interpreter min execution time: {min1}");

        // something about 173
        var min2 = Measure(
            () => new ToMsilTranslator.ToMsilTranslator(),
            repeatTimes, code
        );
        Console.WriteLine($"Translator to msil min execution time: {min2}");

        // something about 9,566473988439306
        Console.WriteLine($"Translator faster than interpreter in {(double)min1 / min2} times");
    }

    private long Measure(Func<IExecutor> executorMaker, int times, string code)
    {
        var min = long.MaxValue;

        for (var i = 0; i < times; i++)
        {
            var quarkStatistics = new QuarkStatistics();
            var lexemes =
                quarkStatistics.Measure(() => new Lexer(LexerConfiguration.GetPatterns().ToList()).Lexemize(code));
            var asg = quarkStatistics.Measure(() => new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes));
            var module =
                quarkStatistics.Measure(() => new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg));
            var executor = executorMaker();

            var stdOut = Console.Out;
            Console.SetOut(TextWriter.Null);
            quarkStatistics.Measure(() => executor.RunModule(module, [null]));
            Console.SetOut(stdOut);

            min = Math.Min(min, quarkStatistics.Times[^1].Item1);
        }

        return min;
    }
}