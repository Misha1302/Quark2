using CommonBytecode.Enums;

namespace CommonBytecode.Data.Structures;

public record BytecodeInstruction(InstructionType Type, List<Any> Arguments, string Name = "") : INameable
{
    public override string ToString() => $"{Type} {Name} [{string.Join(", ", Arguments)}]";
}