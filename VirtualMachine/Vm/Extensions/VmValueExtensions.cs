using CommonDataStructures;
using VirtualMachine.Vm.Preparing;

namespace VirtualMachine.Vm.Extensions;

public static class VmValueExtensions
{
    public static List<AnyOpt> MakeAnyOptList(this Any any)
    {
        return any.Value switch
        {
            double d => [AnyOpt.Create(d, Number)],
            long l => [AnyOpt.Create(l, NativeI64)],
            bool l => [AnyOpt.Create(l ? 1L : 0L, NativeI64)],
            string s => [AnyOpt.CreateRef(s, Str)],
            Delegate value => [..SharpCallArgs.MakeCallSharpOperationArguments(value)],
            Enum e => [AnyOpt.Create((long)Convert.ToInt32(e), NativeI64)],
            null => [AnyOpt.NilValue],
            var obj => [AnyOpt.CreateRef(obj, SomeSharpObject)],
        };
    }
}