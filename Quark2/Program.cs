using AbstractExecutor;
using CommonBytecode;
using Quark2;
using VirtualMachine;

var fivePowTwoFuncBytecode = (List<Instruction>)
[
    new Instruction(InstructionType.MakeVariables, [new BytecodeVariable("i", Number).ToAny()]),
    new Instruction(InstructionType.PushConst, [5.0]),
    new Instruction(InstructionType.SetLocal, ["i"]),

    new Instruction(InstructionType.LoadLocal, ["i"]),
    new Instruction(InstructionType.PushConst, [2.0]),
    new Instruction(InstructionType.MathOrLogicOp, [MathLogicOp.Pow]),

    new Instruction(InstructionType.CallSharp, [((Action<Any>)BuiltInFunctions.PrintLn).ToAny()]),

    new Instruction(InstructionType.PushConst, [new Any(null!)]),
    new Instruction(InstructionType.Ret, []),
];

var module = new BytecodeModule(
    "five to the second power",
    [new BytecodeFunction("Main", new Bytecode(fivePowTwoFuncBytecode))]
);

IExecutor executor = new QuarkVirtualMachine();
var results = executor.RunModule(module);
Console.WriteLine(string.Join(", ", results));