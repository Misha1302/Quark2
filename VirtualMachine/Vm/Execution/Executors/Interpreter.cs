using static CommonBytecode.Enums.InstructionType;

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
        var opType = vmOperation.Type;
        if (opType == PushConst) Stack.Push(vmOperation.Args[0]);
        else if (opType == MathOrLogicOp) DoMathOrLogicOp(vmOperation.Args[0].Get<MathLogicOp>());
        else if (opType == Ret) Frames.Pop();
        else if (opType == CallSharp) CallSharpFunctionOp(vmOperation);
        else if (opType == CallFunc) CallFunctionOp(vmOperation);
        else if (opType == SetLocal) SetLocalOp(vmOperation);
        else if (opType == LoadLocal) LoadLocalOp(vmOperation);
        else if (opType == Br) BrOp(vmOperation);
        else if (opType == Label) DoNothingOp();
        else if (opType == MakeVariables) DoNothingOp();
        else if (opType == Drop) Stack.Pop();
        else Throw.InvalidOpEx($"Unknown operation {vmOperation}");
    }

    private void CallFunctionOp(VmOperation vmOperation)
    {
        Frames.Push(new VmFuncFrame(_engineRuntimeData.Module.Functions[(int)vmOperation.Args[0].Get<long>()]));
    }

    private void BrOp(VmOperation vmOperation)
    {
        DoBranch(vmOperation.Args[0].Get<BranchMode>(), vmOperation.Args[1].Get<long>());
    }

    private void LoadLocalOp(VmOperation vmOperation)
    {
        Stack.Push(Frames.Get(-1).Variables[(int)vmOperation.Args[0].Get<long>()].Value);
    }

    private void SetLocalOp(VmOperation vmOperation)
    {
        Frames.Get(-1).Variables[(int)vmOperation.Args[0].Get<long>()].Value = Stack.Pop();
    }

    private void CallSharpFunctionOp(VmOperation vmOperation)
    {
        SharpInteractor.CallStaticSharpFunction(
            Stack,
            vmOperation.Args[0].Get<nint>(),
            vmOperation.Args[1].Get<long>(),
            vmOperation.Args[2].IsTrue(),
            vmOperation.Args[3].IsTrue()
        );
    }

    private void DoNothingOp()
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

    private void DoMathOrLogicOp(MathLogicOp op)
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