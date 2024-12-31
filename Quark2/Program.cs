using AbstractExecutor;
using Quark2;
using QuarkCFrontend;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;
using VirtualMachine;

var code2 = File.ReadAllText("Code/Main.lua");


var repeatTimes = 10;

var min1 = Measure(() => new QuarkVirtualMachine(new ExecutorConfiguration([])), repeatTimes);
Console.WriteLine($"Interpreter min execution time: {min1}");

var min2 = Measure(() => new ToMsilTranslator.ToMsilTranslator(new ExecutorConfiguration([])), repeatTimes);
Console.WriteLine($"Translator to msil min execution time: {min2}");

Console.WriteLine($"Translator faster than interpreter in {(double)min1 / min2} times");


long Measure(Func<IExecutor> executorMaker, int times)
{
    var min = long.MaxValue;

    for (var i = 0; i < times; i++)
    {
        var quarkStatistics = new QuarkStatistics();
        var lexemes =
            quarkStatistics.Measure(() => new Lexer(LexerConfiguration.GetPatterns().ToList()).Lexemize(code2));
        var asg = quarkStatistics.Measure(() => new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes));
        var module =
            quarkStatistics.Measure(() => new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg));
        var executor = executorMaker();
        var results = quarkStatistics.Measure(() => executor.RunModule(module, [null]));

        // Console.WriteLine($"Results: {string.Join(", ", results)}");
        // Console.WriteLine("Statistics:");
        // Console.WriteLine(quarkStatistics.ToString());
        min = Math.Min(min, quarkStatistics.Times[^1].Item1);
    }

    return min;
}