using CommonBytecode;

namespace VirtualMachine.Vm.DataStructures.VmValues;

public static class VmValueExtensions
{
    public static List<VmValue> MakeVmValue(this Any any)
    {
        return any.Value switch
        {
            double d => [VmValue.Create(d, Number)],
            long l => [VmValue.Create(l, NativeI64)],
            string s => [VmValue.CreateRef(s, Str)],
            Delegate value => [..SharpCall.MakeCallSharpOperationArguments(value)],
            MathLogicOp op => [VmValue.Create(op, NativeI64)],
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            null => [VmValue.NilValue],
            _ => [Throw.InvalidOpEx<VmValue>()],
        };
    }
}