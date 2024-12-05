using AbstractExecutor;
using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
using SharpLibrariesImporter;
using VirtualMachine;
using static BytecodeGenerationSimplifier.SimpleBytecodeGenerator;
using Any = CommonBytecode.Data.AnyValue.Any;


/*
for number i = 1, i <= 10.0, i = i + 1 {
    number j = Cube(i)
    Print(j)
    Print(" ")
}
PrintLn("")

return Nil
*/

var importsManager = new ImportsManager();
importsManager.Import("../../../../Libraries/bin/net8.0/QuarkIO.dll");

var start = (Func<List<BytecodeInstruction>>)(() =>
[
    ..DefineLocals(("i", Number), ("top", Number)),
    new BytecodeInstruction(InstructionType.PushConst, [1.0]),
    new BytecodeInstruction(InstructionType.SetLocal, ["i"]),

    ..CallSharp(importsManager.GetDelegateByName("InputNumber")),
    new BytecodeInstruction(InstructionType.SetLocal, ["top"]),
]);
var cond = (Func<List<BytecodeInstruction>>)(() =>
[
    new BytecodeInstruction(InstructionType.LoadLocal, ["i"]),
    new BytecodeInstruction(InstructionType.LoadLocal, ["top"]),
    new BytecodeInstruction(InstructionType.MathOrLogicOp, [MathLogicOp.LtOrEq.ToAny()]),
]);
var step = (Func<List<BytecodeInstruction>>)(() => [..Inc("i")]);
var body = (Func<List<BytecodeInstruction>>)(() =>
[
    ..DefineLocals(("j", Number)),
    new BytecodeInstruction(InstructionType.LoadLocal, ["i"]),
    new BytecodeInstruction(InstructionType.SetLocal, ["j"]),

    new BytecodeInstruction(InstructionType.LoadLocal, ["j"]),
    ..CallSharp(importsManager.GetDelegateByName("Print")),
    new BytecodeInstruction(InstructionType.PushConst, [" "]),
    ..CallSharp(importsManager.GetDelegateByName("Print")),
]);

var fivePowTwoFuncBytecode = (List<BytecodeInstruction>)
[
    ..For(start, cond, step, body),

    new BytecodeInstruction(InstructionType.PushConst, [""]),
    ..CallSharp(importsManager.GetDelegateByName("PrintLn")),

    new BytecodeInstruction(InstructionType.PushConst, [Any.Nil]),
    new BytecodeInstruction(InstructionType.Ret, []),
];


var module = new BytecodeModule(
    "print cubes",
    [new BytecodeFunction("Main", new Bytecode(fivePowTwoFuncBytecode))]
);

IExecutor executor = new QuarkVirtualMachine();
var results = executor.RunModule(module, [null]);
Console.WriteLine(string.Join(", ", results));