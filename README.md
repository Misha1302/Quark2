### A Framework for easy and fast creation programming languages and DSLs

The main idea is make many components with common API. It should help create DSLs as constructors from implemented
components and, if necessary, create your own components.

Minimal example of hello world using implemented components:

```C#
const string code =
    """
    import "../../../../Libraries"

    def Main() {
        _ = PrintLn("Hello, World!")
        return 0
    }
    """;

var executor = new TranslatorToMsil.TranslatorToMsil();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor, []);
Console.WriteLine(result);
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

You can create yours extensions for Quark DSL also. 

There is example of using QuarkExtStructures:
```csharp
using QuarkExtension;
using QuarkStructures;

const string code =
    """
    import "../../../../Libraries"

    struct Vector2(x, y)

    def Main() {
        v1 = NewVector2(2, 3)
        v2 = NewVector2(-12, 7)
        v3 = SumVectors(v1, v2)
        _ = PrintLn(v3)
        return 0
    }

    def SumVectors(a, b) {
        c = NewVector2(a->x + b->x, a->y + b->y)
        return c
    }

    def NewVector2(x, y) {
        v = CreateStruct("Vector2")
        v->x = x
        v->y = y
        return v
    }
    """;

var extensions = (List<IQuarkExtension>) [new QuarkExtStructures()];
var executor = new TranslatorToMsil.TranslatorToMsil();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor, extensions);
Console.WriteLine(result);
```