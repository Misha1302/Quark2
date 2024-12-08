namespace CommonBytecode.Data.Structures;

public record Bytecode(List<BytecodeInstruction> Instructions)
{
    public override string ToString() => $"{string.Join("\n", Instructions)}";
}