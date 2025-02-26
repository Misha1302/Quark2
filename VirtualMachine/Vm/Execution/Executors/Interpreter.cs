using StrongAnyValueCalculator;

namespace VirtualMachine.Vm.Execution.Executors;

public class Interpreter
{
    public readonly MyStack<VmFuncFrame> Frames = new(1024);
    public readonly MyStack<AnyOpt> Stack = new(1024);
    private EngineRuntimeData _engineRuntimeData = null!;

    public bool Halted => Frames.Count == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Step(int stepsCount, EngineRuntimeData engineRuntimeData)
    {
        _engineRuntimeData = engineRuntimeData;
        for (var i = 0; i < stepsCount && Frames.Count != 0; i++)
        {
            var func = Frames.Get(-1);
            var op = CollectionsMarshal.AsSpan(func.Ops)[func.Ip];
            ExecuteOp(op);
            _engineRuntimeData.LogAction?.Invoke(op, i, func, Stack);
            func.Ip++;
        }
    }

    private void ExecuteOp(VmOperation vmOperation)
    {
        if (vmOperation.Type == InstructionType.PushConst) Stack.Push(vmOperation.Args[0]);
        else if (vmOperation.Type == InstructionType.MathOrLogicOp)
            DoMathOrLogic(vmOperation.Args[0].Get<MathLogicOp>());
        else if (vmOperation.Type == InstructionType.Ret) Frames.Pop();
        else if (vmOperation.Type == InstructionType.CallSharp) CallSharpFunction(vmOperation);
        else if (vmOperation.Type == InstructionType.CallFunc) CallFunction(vmOperation);
        else if (vmOperation.Type == InstructionType.SetLocal) SetLocal(vmOperation);
        else if (vmOperation.Type == InstructionType.LoadLocal) LoadLocal(vmOperation);
        else if (vmOperation.Type == InstructionType.BrOp) BrOp(vmOperation);
        else if (vmOperation.Type == InstructionType.Label) DoNothing();
        else if (vmOperation.Type == InstructionType.MakeVariables) DoNothing();
        else if (vmOperation.Type == InstructionType.Drop) Stack.Pop();
        else Throw.InvalidOpEx();
    }

    private void CallFunction(VmOperation vmOperation)
    {
        Frames.Push(new VmFuncFrame(_engineRuntimeData.Module.Functions[(int)vmOperation.Args[0].Get<long>()]));
    }

    private void BrOp(VmOperation vmOperation)
    {
        DoBranch(vmOperation.Args[0].Get<BranchMode>(), vmOperation.Args[1].Get<long>());
    }

    private void LoadLocal(VmOperation vmOperation)
    {
        Stack.Push(Frames.Get(-1).Variables[(int)vmOperation.Args[0].Get<long>()].Value);
    }

    private void SetLocal(VmOperation vmOperation)
    {
        Frames.Get(-1).Variables[(int)vmOperation.Args[0].Get<long>()].Value = Stack.Pop();
    }

    private void CallSharpFunction(VmOperation vmOperation)
    {
        SharpInteractor.CallStaticSharpFunction(
            Stack,
            vmOperation.Args[0].Get<nint>(),
            vmOperation.Args[1].Get<long>(),
            vmOperation.Args[2].IsTrue(),
            vmOperation.Args[3].IsTrue()
        );
    }

    private void DoNothing()
    {
    }

    private void DoBranch(BranchMode branchMode, long labelIndex)
    {
        var vmFrame = Frames.Get(-1);

        if (branchMode == BranchMode.Basic)
        {
            vmFrame.Ip = vmFrame.Labels[(int)labelIndex].Ip;
            return;
        }

        var value = Stack.Pop();

        var isTrue = branchMode == BranchMode.IfTrue && value.IsTrue();
        var isFalse = branchMode == BranchMode.IfFalse && value.IsFalse();
        if (isTrue || isFalse)
            vmFrame.Ip = vmFrame.Labels[(int)labelIndex].Ip;
    }

    private void DoMathOrLogic(MathLogicOp op)
    {
        if (op == Not)
        {
            Stack.Push(AnyOptCalculator.Not(Stack.Pop()));
            return;
        }

        var b = Stack.Pop();
        var a = Stack.Pop();
        var c = op switch
        {
            Sum => AnyOptCalculator.Sum(a, b),
            Sub => AnyOptCalculator.Sub(a, b),
            Mul => AnyOptCalculator.Mul(a, b),
            Div => AnyOptCalculator.Div(a, b),
            Pow => AnyOptCalculator.Pow(a, b),
            Mod => AnyOptCalculator.Mod(a, b),
            And => AnyOptCalculator.And(a, b),
            Or => AnyOptCalculator.Or(a, b),
            Xor => AnyOptCalculator.Xor(a, b),
            Eq => AnyOptCalculator.Eq(a, b),
            NotEq => AnyOptCalculator.NotEq(a, b),
            Lt => AnyOptCalculator.Lt(a, b),
            Gt => AnyOptCalculator.Gt(a, b),
            LtOrEq => AnyOptCalculator.LtOrEq(a, b),
            GtOrEq => AnyOptCalculator.GtOrEq(a, b),
            _ => Throw.InvalidOpEx<AnyOpt>(),
        };

        Stack.Push(c);
    }
}