### A Framework for easy and fast creation programming languages and DSLs
The main idea is make many components with common API. It should help create DSLs as constructors from implemented components and, if necessary, create your own components.

Minimal example of hello world using implemented components:

```C#
using AbstractExecutor;
using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;
using QuarkCFrontend;
using VirtualMachine;

const string code =
    """
    import "../../../../Libraries"

    def Main() {
        PrintLn("Hello World!")
        return 0
    }
    """;

var lexemes = new Lexer(LexerDefaultConfiguration.CreateDefault()).Lexemize(code);
var asg = new AsgBuilder(AsgBuilderConfiguration.CreateDefault()).Build(lexemes);
var module = new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg);
var executor = new QuarkVirtualMachine();
executor.Init(new ExecutorConfiguration(module));
executor.RunModule();
```
