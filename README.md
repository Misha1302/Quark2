# A Framework for easy and fast creation programming languages and DSLs
## Main description
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

## Experimental bytecode branch description
### Innovative approach to bytecode

New bytecode arranged like
`List<(Instruction instruction, Dictionary<float, List<(Delegate del, object[] args)>> threadedCode)> bytecode`:

```C#
List<BytecodeInstruction> bytecode;

public record BytecodeInstruction(Instruction Instruction, Dictionary<float, ThreadedFunction> Functions);

public record ThreadedFunction(Delegate Function, object[] Args);
```

The main idea is the generalization of bytecode. E.g., we haven't `setLoc` instruction now, but we have generalized
`loadRef` and `set`. E.g., basic bytecode was:

```
push 5
setLoc i
```

Now it will be like:

```
push [{ 0, { PushConst, [5] }}]             // Stack: 5(int)
loadRef [{ 0, { LoadLocalRef, ["i"] }}]     // Stack: 5(int), i(IVariable)
set [{ 0, { SetIVariable, [] }}]            // Stack empty
```

We do not load reference of local, we do not use concrete instruction to load reference to local. "Extensions" should
analyze bytecode's instructions and add their functions for each necessary instruction. You can notice, `loadRef` loads
`IVariable` instance. It's not a concrete type 'cause we need to save maximum generalization. E.g., you can add
structures, and they will implement `IVariable` to so as not to write new code and new instructions.

`IVariable` will something be like this:

```
public interface IVariable
{
    public void SetValue(IAny value);
    public IAny GetValue();
}
```

### Improved approach to instructions enum

Now only one value by default in `Instruction` enum: Invalid. All others should be got and saved via
`InstructionEnumManager`. Something like this:

```
public enum Instruction { Invalid }

public static class InstructionEnumManager
{
    private static int _curInstructionNumber = (int)Instruction.Invalid + 1;
    public static Instruction GetNextInstruction() => (Instruction)_curInstructionNumber++;
}

class SetInstruction
{
    ...
    public static Instruction Set = InstructionEnumManager.GetNextInstruction()
    ...
}
```