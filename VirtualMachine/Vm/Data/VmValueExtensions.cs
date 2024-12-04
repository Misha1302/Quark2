using CommonBytecode.Data.AnyValue;
using ExceptionsManager;

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
            Delegate value => [..SharpCallArgs.MakeCallSharpOperationArguments(value)],
            Enum e => [VmValue.Create((long)Convert.ToInt32(e), NativeI64)],
            null => [VmValue.NilValue],
            _ => [Throw.InvalidOpEx<VmValue>()],
        };
    }
}