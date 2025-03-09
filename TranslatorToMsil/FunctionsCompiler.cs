namespace TranslatorToMsil;

public class FunctionsCompiler
{
    public void CompileInstruction(
        GroboIL il,
        BytecodeInstruction? prevInstruction,
        BytecodeInstruction instruction,
        BytecodeInstruction? nextInstruction,
        FunctionCompileData data,
        BytecodeModule module,
        List<AnyOpt> constants)
    {
        if (instruction.Type == InstructionType.PushConst)
            PushConst(il, instruction, constants);
        else if (instruction.Type == InstructionType.MathOrLogicOp)
            CompileMathOrLogicOp(il, instruction);
        else if (instruction.Type == InstructionType.LoadLocal)
            il.Ldloc(data.Locals[instruction.Arguments[0].Get<string>()]);
        else if (instruction.Type == InstructionType.SetLocal)
            il.Stloc(data.Locals[instruction.Arguments[0].Get<string>()]);
        else if (instruction.Type == InstructionType.Br)
            CompileBrOp(il, instruction, data);
        else if (instruction.Type == InstructionType.CallSharp)
            CallSharp(il, instruction, prevInstruction ?? Throw.InvalidOpEx<BytecodeInstruction>());
        else if (instruction.Type == InstructionType.CallFunc)
            CallFunc(il, instruction, nextInstruction, module);
        else if (instruction.Type == InstructionType.Ret)
            il.Ret();
        else if (instruction.Type == InstructionType.Label)
            il.MarkLabel(data.Labels[instruction.Arguments[0].Get<string>()]);
        else if (instruction.Type == InstructionType.Drop)
            il.Pop();
        else if (instruction.Type == InstructionType.MakeVariables)
            DoNothing();
    }

    private void CallSharp(GroboIL il, BytecodeInstruction instruction, BytecodeInstruction prevInstruction)
    {
        var method = instruction.Arguments[0].Get<Delegate>().GetInfo();
        var parameters = method.GetParameters();

        var isVarArgs = parameters.Any(x => x.ParameterType == typeof(IReadOnlyStack<Any>));
        var size = !isVarArgs ? parameters.Length : prevInstruction.Arguments[0].Get<double>().ToLong() + 1;

        var locals = Enumerable.Range(0, (int)size).Select(_ => il.DeclareLocal(typeof(Any))).ToList();
        foreach (var local in locals)
        {
            il.Box(typeof(AnyOpt));
            il.Castclass(typeof(IAny));
            il.Call(DelegatesHelper.GetInfo(AnyExtensions.ToAny));
            il.Stloc(local);
        }

        if (!isVarArgs)
        {
            for (var i = locals.Count - 1; i >= 0; i--) il.Ldloc(locals[i]);

            il.Call(method);
        }
        else
        {
            // Any[] args, nint ptr, bool returnsValue

            var arrLoc = il.DeclareLocal(typeof(Any[]));
            il.Ldc_I4((int)size);
            il.Newarr(typeof(Any));
            il.Stloc(arrLoc);
            for (var i = 0; i < locals.Count; i++)
            {
                il.Ldloc(arrLoc);
                il.Ldc_I4(i);
                il.Ldelema(typeof(Any));
                il.Ldloc(locals[^(i + 1)]);
                il.Stobj(typeof(Any));
            }

            il.Ldloc(arrLoc);
            il.Ldc_IntPtr(method.MethodHandle.GetFunctionPointer());
            il.Ldc_I4(method.ReturnType != typeof(void) ? 1 : 0);
            il.Call(DelegatesHelper.GetInfo(MsilSharpInteractioner.CallVarArgsStaticSharpFunction));
        }

        if (method.ReturnType != typeof(void))
            il.Call(DelegatesHelper.GetInfo(AnyOptExtensions.MakeAnyOpt));
        else il.Ldfld(typeof(AnyOpt).GetField(nameof(AnyOpt.NilValue)));
    }

    private void PushConst(GroboIL il, BytecodeInstruction instruction, List<AnyOpt> constants)
    {
        constants.Add(instruction.Arguments[0].MakeAnyOpt());
        il.Ldc_I4(constants.Count - 1);
        il.Call(DelegatesHelper.GetInfo(RuntimeLibrary.GetConst));
    }

    private void DoNothing()
    {
    }

    private void CallFunc(GroboIL il, BytecodeInstruction instruction, BytecodeInstruction? nextInstruction,
        BytecodeModule module)
    {
        var name = instruction.Arguments[0].Get<string>();
        var func = module.Functions.FirstOrDefault(x => x.Name == name);
        Throw.AssertAlways(func != null, $"Function {name} not found");
        var parametersCount = func.Code.GetParametersCount();

        for (var i = 0; i < parametersCount; i++)
        {
            var a = il.DeclareLocal(typeof(AnyOpt));
            il.Stloc(a);
            il.Ldloca(a);
            il.Call(DelegatesHelper.GetInfo(RuntimeLibrary.PushToStack));
        }

        il.Ldstr(name);
        il.Call(DelegatesHelper.GetInfo(RuntimeLibrary.CallFunc),
            tailcall: nextInstruction?.Type == InstructionType.Ret);
    }

    private void CompileBrOp(GroboIL il, BytecodeInstruction instruction, FunctionCompileData data)
    {
        var name = instruction.Arguments[1].Get<string>();
        var branch = instruction.Arguments[0].Get<BranchMode>();
        var label = data.Labels[name];

        switch (branch)
        {
            case BranchMode.Basic:
                il.Br(label);
                break;
            case BranchMode.IfTrue:
                PushRefsFromStack(1, il);
                il.Call(DelegatesHelper.GetInfo(RuntimeLibrary.ToBool));
                il.Brtrue(label);
                break;
            case BranchMode.IfFalse:
                PushRefsFromStack(1, il);
                il.Call(DelegatesHelper.GetInfo(RuntimeLibrary.ToBool));
                il.Brfalse(label);
                break;
            default:
                Throw.InvalidOpEx();
                break;
        }
    }

    private void CompileMathOrLogicOp(GroboIL il, BytecodeInstruction instruction)
    {
        var op = instruction.Arguments[0].Get<MathLogicOp>();

        var c = op switch
        {
            Sum => AnyOptCalculator.Sum,
            Sub => AnyOptCalculator.Sub,
            Mul => AnyOptCalculator.Mul,
            Div => AnyOptCalculator.Div,
            Pow => AnyOptCalculator.Pow,
            Mod => AnyOptCalculator.Mod,
            And => AnyOptCalculator.And,
            Or => AnyOptCalculator.Or,
            Xor => AnyOptCalculator.Xor,
            Eq => AnyOptCalculator.Eq,
            NotEq => AnyOptCalculator.NotEq,
            Lt => AnyOptCalculator.Lt,
            Gt => AnyOptCalculator.Gt,
            LtOrEq => AnyOptCalculator.LtOrEq,
            GtOrEq => AnyOptCalculator.GtOrEq,
            Not => AnyOptCalculator.Not,
            _ => Throw.InvalidOpEx<Delegate>(),
        };

        PushRefsFromStack(op == Not ? 1 : 2, il);
        il.Call(c.GetInfo());
    }

    private void PushRefsFromStack(int count, GroboIL il)
    {
        var locals = Enumerable.Range(0, count).Select(_ => il.DeclareLocal(typeof(AnyOpt))).ToArray();
        foreach (var local in locals) il.Stloc(local);
        foreach (var local in locals.Reverse()) il.Ldloca(local);
    }
}