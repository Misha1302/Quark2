### A Framework for easy and fast creation programming languages and DSLs
The main idea is make many components with common API. It should help create DSLs as constructors from implemented components and, if necessary, create your own components.

Minimal example of hello world using implemented components:

```C#
using QuarkCFrontend;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;


const string code =
    """
    import "../../../../Libraries"

    def Main() {
        PrintLn("Hello, World!")
        return 0
    }
    """;

var lexemes = new Lexer(LexerConfiguration.Default).Lexemize(code);
var asg = new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes);
var module = new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg);
var executor = new ToMsilTranslator.ToMsilTranslator();
var results = executor.RunModule(module);

Console.WriteLine($"Results: {string.Join(", ", results)}");
```
