namespace CommonBytecode.Data.Structures;

public record Bytecode(List<BytecodeInstruction> Instructions)
{
    public override string ToString() => $"{string.Join("\n", Instructions)}";

    public int GetParametersCount()
    {
        Throw.AssertDebug(Instructions.Count != 0);
        Throw.AssertDebug(Instructions[0].Type == InstructionType.MakeVariables);
        return Instructions[0].Arguments.Count;
    }
}