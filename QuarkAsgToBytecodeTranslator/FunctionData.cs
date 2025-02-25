namespace AsgToBytecodeTranslator;

public record FunctionData(
    BytecodeFunction BytecodeFunction,
    List<BytecodeVariable> Parameters,
    List<BytecodeVariable> Locals
);