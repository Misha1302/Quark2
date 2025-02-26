using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
using SharpAnyType;

namespace BytecodeGenerationSimplifier;

public static class SimpleBytecodeGenerator
{
    public static List<BytecodeInstruction> For(
        Func<List<BytecodeInstruction>> start,
        Func<List<BytecodeInstruction>> cond,
        Func<List<BytecodeInstruction>> step,
        Func<List<BytecodeInstruction>> body
    )
    {
        var instructions = new List<BytecodeInstruction>();
        instructions.AddRange(start());
        instructions.AddRange(While(cond, () => body().Concat(step()).ToList()));

        return instructions;
    }

    public static List<BytecodeInstruction> While(
        Func<List<BytecodeInstruction>> cond,
        Func<List<BytecodeInstruction>> body
    )
    {
        var instructions = new List<BytecodeInstruction>();

        var startLoop = Guid.NewGuid().ToString();
        var endLoop = Guid.NewGuid().ToString();

        instructions.Add(new BytecodeInstruction(InstructionType.Label, [startLoop]));
        instructions.AddRange(cond());
        instructions.Add(new BytecodeInstruction(InstructionType.BrOp, [BranchMode.IfFalse.ObjectToAny(), endLoop]));

        instructions.AddRange(body());

        instructions.Add(new BytecodeInstruction(InstructionType.BrOp, [BranchMode.Basic.ObjectToAny(), startLoop]));
        instructions.Add(new BytecodeInstruction(InstructionType.Label, [endLoop]));

        return instructions;
    }

    public static void While(Action cond, Action body, Bytecode bytecode)
    {
        var startLoop = Guid.NewGuid().ToString();
        var endLoop = Guid.NewGuid().ToString();

        bytecode.Instructions.Add(new BytecodeInstruction(InstructionType.Label, [startLoop]));
        cond();
        bytecode.Instructions.Add(new BytecodeInstruction(InstructionType.BrOp, [BranchMode.IfFalse.ObjectToAny(), endLoop]));

        body();

        bytecode.Instructions.Add(new BytecodeInstruction(InstructionType.BrOp, [BranchMode.Basic.ObjectToAny(), startLoop]));
        bytecode.Instructions.Add(new BytecodeInstruction(InstructionType.Label, [endLoop]));
    }

    public static List<BytecodeInstruction> Inc(string name) =>
    [
        new(InstructionType.LoadLocal, [name]),
        new(InstructionType.PushConst, [1.0]),
        new(InstructionType.MathOrLogicOp, [MathLogicOp.Sum.ObjectToAny()]),
        new(InstructionType.SetLocal, [name]),
    ];

    public static List<BytecodeInstruction> CallSharp(Delegate printLn) =>
        [new(InstructionType.CallSharp, [printLn.ObjectToAny(AnyValueType.SomeSharpObject)])];

    public static List<BytecodeInstruction> DefineLocals(params (string, AnyValueType)[] locals)
    {
        return
        [
            new BytecodeInstruction(InstructionType.MakeVariables,
                locals.Select(x => new BytecodeVariable(x.Item1, x.Item2).ObjectToAny()).ToList()),
        ];
    }

    public static List<BytecodeInstruction> ReadParameters(params string[] parameters)
    {
        return parameters.Reverse().Select(x => new BytecodeInstruction(InstructionType.SetLocal, [x])).ToList();
    }

    public static void For(Action start, Action cond, Action step, Action body, Bytecode bytecode)
    {
        start();
        While(cond, () =>
        {
            body();
            step();
        }, bytecode);
    }

    public static void If(Action cond, Action body, Bytecode bytecode)
    {
        cond();
        var elseLabel = Guid.NewGuid().ToString();
        bytecode.Instructions.Add(
            new BytecodeInstruction(InstructionType.BrOp, [BranchMode.IfFalse.ObjectToAny(), elseLabel]));

        body();

        bytecode.Instructions.Add(new BytecodeInstruction(InstructionType.Label, [elseLabel]));
    }
}