using VirtualMachine.Vm.Preparing;

namespace VirtualMachine.Vm.Extensions;

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
            var obj => [VmValue.CreateRef(obj, SomeSharpObject)],
        };
    }

    public static object GetValueInSharpType(this VmValue value)
    {
        return value.Type switch
        {
            Nil => "Nil",
            Number => value.Get<double>(),
            Str => value.GetRef<string>(),
            SomeSharpObject => value.GetRef<object>(),
            NativeI64 => value.Get<long>(),
            BytecodeValueType.Any => value.Get<long>(),
            _ => Throw.InvalidOpEx<string>(),
        };
    }
}