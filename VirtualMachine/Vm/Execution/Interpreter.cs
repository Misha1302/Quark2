using CommonBytecode;
using VirtualMachine.Vm.DataStructures;
using VirtualMachine.Vm.DataStructures.VmValues;
using VirtualMachine.Vm.Operations;
using static CommonBytecode.MathLogicOp;

namespace VirtualMachine.Vm.Execution;

public class Interpreter
{
    public readonly Stack<VmFuncFrame> Frames = new();
    public readonly MyStack<VmValue> Stack = new();
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

    public void Step(int stepsCount, EngineRuntimeData engineRuntimeData)
    {
        _engineRuntimeData = engineRuntimeData;
        for (var i = 0; i < stepsCount && Frames.Count != 0; i++)
        {
            var func = Frames.Peek();
            var op = func.Ops[func.Ip];
            ExecuteOp(op);
            _engineRuntimeData.LogAction?.Invoke(op, i, func, Stack);
            func.Ip++;
        }
    }

    private void ExecuteOp(Operation operation)
    {
        if (operation.Type == InstructionType.PushConst) Stack.Push(operation.Args[0]);
        else if (operation.Type == InstructionType.MathOrLogicOp) DoMathOrLogic(operation.Args[0].Get<MathLogicOp>());
        else if (operation.Type == InstructionType.Ret) Frames.Pop();
        else if (operation.Type == InstructionType.CallSharp) CallSharpFunction(operation);
        else if (operation.Type == InstructionType.CallFunc) CallFunction(operation);
        else if (operation.Type == InstructionType.SetLocal) SetLocal(operation);
        else if (operation.Type == InstructionType.LoadLocal) LoadLocal(operation);
        else if (operation.Type == InstructionType.BrOp) BrOp(operation);
        else if (operation.Type == InstructionType.Label) DoNothing();
        else if (operation.Type == InstructionType.MakeVariables) DoNothing();
        else if (operation.Type == InstructionType.Drop) Stack.Pop();
        else Throw.InvalidOpEx();
    }

    private void CallFunction(Operation operation)
    {
        Frames.Push(new VmFuncFrame(_engineRuntimeData.Module.Functions[(int)operation.Args[0].Get<long>()]));
    }

    private void BrOp(Operation operation)
    {
        DoBranch(operation.Args[0].Get<BranchMode>(), operation.Args[1].Get<long>());
    }

    private void LoadLocal(Operation operation)
    {
        Stack.Push(Frames.Peek().Variables[(int)operation.Args[0].Get<long>()].Value);
    }

    private void SetLocal(Operation operation)
    {
        Frames.Peek().Variables[(int)operation.Args[0].Get<long>()].Value = Stack.Pop();
    }

    private void CallSharpFunction(Operation operation)
    {
        SharpInteractioner.CallStaticSharpFunction(
            Stack,
            operation.Args[0].Get<nint>(),
            operation.Args[1].Get<long>(),
            operation.Args[2].IsTrue()
        );
    }

    private void DoNothing()
    {
    }

    private void DoBranch(BranchMode branchMode, long labelIndex)
    {
        var vmFrame = Frames.Peek();

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
            And => VmCalc.And(a, b),
            Or => VmCalc.Or(a, b),
            Xor => VmCalc.Xor(a, b),
            Eq => VmCalc.Eq(a, b, NumbersCompareAccuracy),
            NotEq => VmCalc.NotEq(a, b, NumbersCompareAccuracy),
            _ => Throw.InvalidOpEx<VmValue>(),
        };

        Stack.Push(c);
    }
}