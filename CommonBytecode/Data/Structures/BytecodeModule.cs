namespace CommonBytecode.Data.Structures;

public record BytecodeModule(string Name, List<BytecodeFunction> Functions);