using CommonBytecode.Enums;
using CommonDataStructures;

namespace VirtualMachine.Vm.Data;

public readonly struct VmOperation(InstructionType type, List<AnyOpt> args)
{
    public readonly InstructionType Type = type;
    public readonly List<AnyOpt> Args = args;

    public override string ToString() => $"{Type} [{string.Join(", ", Args)}]";
}