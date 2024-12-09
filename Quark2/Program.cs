using System.Diagnostics;
using System.Runtime.CompilerServices;
using AbstractExecutor;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;
using VirtualMachine;

var code2 =
    """
    import "../../../../Libraries"

    Number Main() {
        PrintLn(2 + 3 ** 5)
    
        return 0
    }
    """;


var quarkStatistics = new QuarkStatistics();
var lexemes = quarkStatistics.Measure(() => new Lexer().Lexemize(code2));
var asg = quarkStatistics.Measure(() => new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes));
// Console.WriteLine(asg);
var module = quarkStatistics.Measure(() => new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg));
var executor = (IExecutor)new QuarkVirtualMachine();
Console.WriteLine(module);
var results = quarkStatistics.Measure(() => executor.RunModule(module, [null]));

Console.WriteLine($"Results: {string.Join(", ", results)}");
Console.WriteLine("Statistics:");
Console.WriteLine(quarkStatistics.ToString());


public class QuarkStatistics
{
    private readonly List<(long, string)> _times = [];

    public T Measure<T>(Func<T> func, [CallerArgumentExpression(nameof(func))] string expression = null!)
    {
        var sw = Stopwatch.StartNew();
        var result = func();
        _times.Add((sw.ElapsedMilliseconds, expression));
        return result;
    }

    public override string ToString() =>
        string.Join("\n", _times.Select(x => $"{x.Item1}".PadRight(6) + $": {x.Item2}"));
}