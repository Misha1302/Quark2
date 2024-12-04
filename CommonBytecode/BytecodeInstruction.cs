namespace CommonBytecode;

public record BytecodeInstruction(InstructionType Type, List<Any> Arguments, string Name = "") : INameable;