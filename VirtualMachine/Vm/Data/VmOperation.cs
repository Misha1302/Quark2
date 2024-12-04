using CommonBytecode.Enums;

namespace VirtualMachine.Vm.Data;

public readonly struct VmOperation(InstructionType type, List<VmValue> args)
{
    public readonly InstructionType Type = type;
    public readonly List<VmValue> Args = args;

    public override string ToString() => $"{Type} [{string.Join(", ", Args)}]";
}