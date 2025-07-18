using CommonBytecode;
using SharpAnyType;

namespace Quark2;

public class Measurer
{
    public void Run(int repeatTimes)
    {
        var code = File.ReadAllText("Code/Main.lua");

        GetExecutionTimes(repeatTimes, code, out var times1, out var times2);
        PrintExecutionStatistics(times1, times2);
    }

    private void PrintExecutionStatistics(List<long> times1, List<long> times2)
    {
        // something about 9,566473988439306
        Console.WriteLine(
            $"Translator faster than interpreter in {(double)times1.Min() / times2.Min()} times (if we take the minimum execution time)");

        Console.WriteLine(
            $"Translator average faster than interpreter in {times1.Average() / times2.Average()} times (if we take the average execution time)");
    }

    private void GetExecutionTimes(int repeatTimes, string code, out List<long> times1, out List<long> times2)
    {
        // something about 1655
        times1 = Measure(
            () => new QuarkVirtualMachine(),
            repeatTimes, code
        );
        Console.WriteLine($"Interpreter min execution time: {times1.Min()} ms");

        // something about 173
        times2 = Measure(
            () => new TranslatorToMsil.TranslatorToMsil(),
            repeatTimes, code
        );
        Console.WriteLine($"Translator to msil min execution time: {times2.Min()} ms");
    }

    private List<long> Measure(Func<IExecutor<ExecutorConfiguration, IEnumerable<Any>>> executorMaker, int times,
        string code)
    {
        var executionTimes = new List<long>(times);

        // + 1 'cause we need to prerun code for best perfomance
        for (var i = 0; i < times + 1; i++)
        {
            var quarkStatistics = new QuarkStatistics();
            var lexemes =
                quarkStatistics.Measure(() =>
                    new QuarkLexer(QuarkLexerDefaultConfiguration.CreateDefault()).Lexemize(code));
            var asg = quarkStatistics.Measure(() =>
                new AsgBuilder<QuarkLexemeType>(QuarkAsgBuilderConfiguration.CreateDefault()).Build(lexemes));
            var module =
                quarkStatistics.Measure(() => new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg));
            var executor = executorMaker();
            executor.Init(new ExecutorConfiguration(module));

            var stdOut = Console.Out;
            Console.SetOut(TextWriter.Null);
            quarkStatistics.Measure(() => executor.RunModule());
            Console.SetOut(stdOut);

            if (i != 0)
                executionTimes.Add(quarkStatistics.Times[^1].Item1);
        }

        return executionTimes;
    }
}