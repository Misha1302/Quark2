### A Framework for easy and fast creation programming languages and DSLs
The main idea is make many components with common API. It should help create DSLs as constructors from implemented components and, if necessary, create your own components.

Minimal example of hello world using implemented components:

```C#
using AbstractExecutor;
using AsgToBytecodeTranslator;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkCFrontend;

const string code =
    """
    import "../../../../Libraries"

    def Main() {
        _ = PrintLn("Hello World!")
        return 0
    }
    """;

var lexemes = new Lexer(QuarkLexerDefaultConfiguration.CreateDefault()).Lexemize(code);
var asg = new AsgBuilder<QuarkLexemeType>(QuarkAsgBuilderConfiguration.CreateDefault()).Build(lexemes);
var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg);
var executor = new TranslatorToMsil.TranslatorToMsil();
executor.Init(new ExecutorConfiguration(module));
var results = executor.RunModule();
Console.WriteLine(results.First());
```
