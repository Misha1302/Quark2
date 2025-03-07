namespace AsgToBytecodeTranslator;

// ReSharper disable NotAccessedPositionalProperty.Global
/// <summary>
///     Contains all data to translate ASG (AST) into common bytecode
/// </summary>
/// <param name="CurBytecode">Bytecode of current function</param>
/// <param name="Node">Current node of ASG to translate</param>
/// <param name="ImportsManager">Object to import libraries</param>
/// <param name="Functions">Existing functions</param>
/// <param name="FunctionsStack">Translating functions</param>
/// <param name="AsgToBytecodeTranslator">Translator asg to bytecode</param>
/// <typeparam name="T">LexemeType of AsgNode</typeparam>
public record AsgToBytecodeData<T>(
    List<BytecodeInstruction> CurBytecode,
    AsgNode<T> Node,
    ImportsManager ImportsManager,
    List<FunctionData> Functions,
    Stack<FunctionData> FunctionsStack,
    IAsgToBytecodeTranslator<T> AsgToBytecodeTranslator
);