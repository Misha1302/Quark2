namespace AsgToBytecodeTranslator;

public record AsgToBytecodeData<T>(
    List<BytecodeInstruction> CurBytecode,
    AsgNode<T> Node,
    ImportsManager ImportsManager,
    List<FunctionData> Functions,
    Stack<FunctionData> FunctionsStack,
    IAsgToBytecodeTranslator<T> AsgToBytecodeTranslator
);