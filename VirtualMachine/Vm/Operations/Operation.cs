using CommonBytecode;
using VirtualMachine.Vm.DataStructures.VmValues;

namespace VirtualMachine.Vm.Operations;

public readonly struct Operation(InstructionType type, List<VmValue> args)
{
    public readonly InstructionType Type = type;
    public readonly List<VmValue> Args = args;

    public override string ToString() => $"{Type} [{string.Join(", ", Args)}]";
}