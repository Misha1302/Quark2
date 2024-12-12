using AbstractExecutor;
using Quark2;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;
using VirtualMachine;

var code2 =
    """
    import "../../../../Libraries"

    Number Main() {
        q = 2
        for (i = 0) (i < 500) (i = i + 1) { 
            for (j = 0) (j < 10000) (j = j + 1) { 
                q = q + 1
            }
        }
        
        PrintLn(q)
    
        return 0
    }

    Number CheckPrime(n) {
        return 1444444444
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