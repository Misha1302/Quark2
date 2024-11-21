namespace CommonBytecode;

public record Instruction(InstructionType Type, List<Any> Arguments, string Name = "") : INameable;