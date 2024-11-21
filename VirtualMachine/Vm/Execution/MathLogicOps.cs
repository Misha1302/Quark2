using CommonBytecode;
using VirtualMachine.Vm.DataStructures.VmValues;

namespace VirtualMachine.Vm.Execution;

public static class MathLogicOps
{
    public static VmValue Sum => VmValue.Create(MathLogicOp.Sum, NativeI64);
    public static VmValue Sub => VmValue.Create(MathLogicOp.Sub, NativeI64);
    public static VmValue Mul => VmValue.Create(MathLogicOp.Mul, NativeI64);
    public static VmValue Div => VmValue.Create(MathLogicOp.Div, NativeI64);
    public static VmValue Pow => VmValue.Create(MathLogicOp.Pow, NativeI64);
    public static VmValue And => VmValue.Create(MathLogicOp.And, NativeI64);
    public static VmValue Or => VmValue.Create(MathLogicOp.Or, NativeI64);
    public static VmValue Xor => VmValue.Create(MathLogicOp.Xor, NativeI64);
    public static VmValue Not => VmValue.Create(MathLogicOp.Not, NativeI64);
    public static VmValue Eq => VmValue.Create(MathLogicOp.Eq, NativeI64);
    public static VmValue NotEq => VmValue.Create(MathLogicOp.NotEq, NativeI64);
}