### A Framework for easy and fast creation programming languages and DSLs

The main idea is make many components with common API. It should help create DSLs as constructors from implemented
components and, if necessary, create your own components.

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

In current moment 9 modules was implemented:

1. CommonLexer (Lexemize code)
2. CommonAst (Parser, build AST)
3. Any (Abstract value of any type)
4. AnyOpt (As any but have fast operation via native 8-byte values)
5. Calculator (Calculator of AnyOpt with strong type system)
6. CommonBytecode (Abstract high-level bytecode)
7. QuarkVirtualMachine (aka QVM, Interpreter of CommonBytecode)
8. TranslatorToMsil (Translate bytecode to msil, compile it and execute)
9. ImportsManager (Helps to import dlls and it's dependencies)