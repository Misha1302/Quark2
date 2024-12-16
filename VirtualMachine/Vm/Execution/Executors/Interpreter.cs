using CommonBytecode.Enums;
using Doubles;

namespace VirtualMachine.Vm.Execution.Executors;

public class Interpreter
{
    public readonly MyStack<VmFuncFrame> Frames = new(1024);
    public readonly MyStack<VmValue> Stack = new(1024);
    private EngineRuntimeData _engineRuntimeData = null!;

    private double _numbersCompareAccuracy = 0.00001;

    public bool Halted => Frames.Count == 0;

    public double NumbersCompareAccuracy
    {
        get => _numbersCompareAccuracy;
        set
        {
            if (value <= 0.0)
                Throw.InvalidOpEx();
            _numbersCompareAccuracy = value;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
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

    public VmValue ExecuteFunction(string name, Span<VmValue> args, EngineRuntimeData engineRuntimeData)
    {
        Stack.PushMany(args);
        Frames.Clear();
        Frames.Push(
            new VmFuncFrame(engineRuntimeData.Module.Functions.First(x => x.Name == name))
        );
        Step(int.MaxValue, engineRuntimeData);
        return Stack.Pop();
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
        else if (vmOperation.Type == InstructionType.PlatformCall) PlatformCall(vmOperation);
        else Throw.InvalidOpEx();
    }

    private void PlatformCall(VmOperation vmOperation)
    {
        var argsCount = Stack.Pop().Get<double>().ToLong();
        var callName = Stack.Get((int)-argsCount - 1).GetRef<string>();
        if (callName == "CallFunction")
        {
            var funcToCall = Stack.Get((int)-argsCount).GetRef<string>();
            Frames.Push(new VmFuncFrame(_engineRuntimeData.Module.Functions.First(x => x.Name == funcToCall)));
        }
        else if (callName == "GetExecutorInfo")
        {
            Stack.Push(VmValue.CreateRef("Interpreter v0.0.0", Str));
        }
        else if (_engineRuntimeData.Configuration.BuildInFunctions.TryGetValue(callName, out var func))
        {
            func(new Any(_engineRuntimeData));
        }
        else
        {
            Throw.InvalidOpEx("Unknown platform call");
        }
    }

    private void CallFunction(VmOperation vmOperation)
    {
        // TODO: remake to pool of vmfuncframe
        Frames.Push(new VmFuncFrame(_engineRuntimeData.Module.Functions[(int)vmOperation.Args[0].Get<long>()]));
        // Console.WriteLine(Frames.Count);
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
        SharpInteractioner.CallStaticSharpFunction(
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
            Stack.Push(VmCalc.Not(Stack.Pop()));
            return;
        }

        var b = Stack.Pop();
        var a = Stack.Pop();
        var c = op switch
        {
            Sum => VmCalc.Sum(a, b),
            Sub => VmCalc.Sub(a, b),
            Mul => VmCalc.Mul(a, b),
            Div => VmCalc.Div(a, b),
            Pow => VmCalc.Pow(a, b),
            Mod => VmCalc.Mod(a, b),
            And => VmCalc.And(a, b),
            Or => VmCalc.Or(a, b),
            Xor => VmCalc.Xor(a, b),
            Eq => VmCalc.Eq(a, b, NumbersCompareAccuracy),
            NotEq => VmCalc.NotEq(a, b, NumbersCompareAccuracy),
            Lt => VmCalc.Lt(a, b),
            Gt => VmCalc.Gt(a, b),
            LtOrEq => VmCalc.LtOrEq(a, b, NumbersCompareAccuracy),
            GtOrEq => VmCalc.GtOrEq(a, b, NumbersCompareAccuracy),
            _ => Throw.InvalidOpEx<VmValue>(),
        };

        Stack.Push(c);
    }
}