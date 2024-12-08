using AbstractExecutor;
using QuarkCFrontend;
using VirtualMachine;

const string code2 =
    """
    import "../../../../Libraries"

    Number Main() {
        PrintLn("Hello, World!")
        PrintLn(2)
        return 6.87
    }
    """;


var lexemes = new Lexer().Lexemize(code2);
var asg = new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes);
Console.WriteLine(asg.ToString());
var module = new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg);
var executor = (IExecutor)new QuarkVirtualMachine();
var results = executor.RunModule(module, [null]);
Console.WriteLine(string.Join(", ", results));
