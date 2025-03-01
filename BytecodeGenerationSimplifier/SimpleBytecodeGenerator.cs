namespace BytecodeGenerationSimplifier;

using BI = BytecodeInstruction;

public static class SimpleBytecodeGenerator
{
    public static List<BI> For(
        Func<List<BI>> start,
        Func<List<BI>> cond,
        Func<List<BI>> step,
        Func<List<BI>> body
    )
    {
        var instructions = new List<BI>();
        instructions.AddRange(start());
        instructions.AddRange(While(cond, () => body().Concat(step()).ToList()));

        return instructions;
    }

    public static List<BI> While(
        Func<List<BI>> cond,
        Func<List<BI>> body
    )
    {
        var instructions = new List<BI>();

        var startLoop = Guid.NewGuid().ToString();
        var endLoop = Guid.NewGuid().ToString();

        instructions.Add(new BI(InstructionType.Label, [startLoop]));
        instructions.AddRange(cond());
        instructions.Add(new BI(InstructionType.Br, [BranchMode.IfFalse.ObjectToAny(), endLoop]));

        instructions.AddRange(body());

        instructions.Add(new BI(InstructionType.Br, [BranchMode.Basic.ObjectToAny(), startLoop]));
        instructions.Add(new BI(InstructionType.Label, [endLoop]));

        return instructions;
    }

    public static void While(Action cond, Action body, Bytecode bytecode)
    {
        var startLoop = Guid.NewGuid().ToString();
        var endLoop = Guid.NewGuid().ToString();

        bytecode.Instructions.Add(new BI(InstructionType.Label, [startLoop]));
        cond();
        bytecode.Instructions.Add(new BI(InstructionType.Br,
            [BranchMode.IfFalse.ObjectToAny(), endLoop]));

        body();

        bytecode.Instructions.Add(new BI(InstructionType.Br,
            [BranchMode.Basic.ObjectToAny(), startLoop]));
        bytecode.Instructions.Add(new BI(InstructionType.Label, [endLoop]));
    }

    public static List<BI> Inc(string name) =>
    [
        new(InstructionType.LoadLocal, [name]),
        new(InstructionType.PushConst, [1.0]),
        new(InstructionType.MathOrLogicOp, [MathLogicOp.Sum.ObjectToAny()]),
        new(InstructionType.SetLocal, [name]),
    ];

    public static List<BI> CallSharp(Delegate printLn) =>
        [new(InstructionType.CallSharp, [printLn.ObjectToAny(AnyValueType.SomeSharpObject)])];

    public static List<BI> DefineLocals(params (string, AnyValueType)[] locals)
    {
        return
        [
            new BI(InstructionType.MakeVariables,
                locals.Select(x => new BytecodeVariable(x.Item1, x.Item2).ObjectToAny()).ToList()),
        ];
    }

    public static List<BI> ReadParameters(params string[] parameters)
    {
        return parameters.Reverse().Select(x => new BI(InstructionType.SetLocal, [x])).ToList();
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

    public static void If(Action cond, Action body, Action elifOrElseBody, Bytecode bytecode)
    {
        var elseLabel = Guid.NewGuid().ToString();
        var endLabel = Guid.NewGuid().ToString();


        cond();

        bytecode.Instructions.Add(new BI(InstructionType.Br, [BranchMode.IfFalse.ObjectToAny(), elseLabel]));

        body();

        bytecode.Instructions.Add(new BI(InstructionType.Br, [BranchMode.Basic.ObjectToAny(), endLabel]));
        bytecode.Instructions.Add(new BI(InstructionType.Label, [elseLabel]));
        elifOrElseBody();
        bytecode.Instructions.Add(new BI(InstructionType.Label, [endLabel]));
    }
}