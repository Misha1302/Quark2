using CommonBytecode;
using VirtualMachine.Vm.DataStructures.VmValues;

namespace VirtualMachine;

public static class VmAnyExtensions
{
    public static object GetValueInSharpType(this VmValue value)
    {
        return value.Type switch
        {
            Nil => "Nil",
            Number => value.Get<double>(),
            Str => value.GetRef<string>(),
            Map => Throw.InvalidOpEx<string>(),
            List => string.Join(", ", value.GetRef<List<BytecodeValue>>()),
            VmFunction => value.GetRef<BytecodeFunction>(),
            SharpFunctionAddress => value.Get<nint>(),
            NativeI64 => value.Get<long>(),
            BytecodeValueType.Any => value.Get<long>(),
            _ => Throw.InvalidOpEx<string>(),
        };
    }
}