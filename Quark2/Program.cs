using AbstractExecutor;
using Quark2;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;
using VirtualMachine;

var code2 =
    """
    import "../../../../Libraries"

    Number Main() {
        n = InputNumber()
    
        found_divisors = 0
    
        for (i = 2) (i < n) (i = i + 1) { 
            if n % i == 0 {
                Print(n)
                Print(" divides by ")
                Print(i)
                PrintLn("")
                
                found_divisors = 1
            }
        }
        
        if (not found_divisors) {
            PrintLn("n is prime")
        }
    
        return 0
    }
    """;


var quarkStatistics = new QuarkStatistics();
var lexemes = quarkStatistics.Measure(() => new Lexer().Lexemize(code2));
var asg = quarkStatistics.Measure(() => new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes));
Console.WriteLine(asg);
var module = quarkStatistics.Measure(() => new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg));
var executor = (IExecutor)new QuarkVirtualMachine();
Console.WriteLine(module);
var results = quarkStatistics.Measure(() => executor.RunModule(module, [null]));

Console.WriteLine($"Results: {string.Join(", ", results)}");
Console.WriteLine("Statistics:");
Console.WriteLine(quarkStatistics.ToString());