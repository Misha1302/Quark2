namespace CommonBytecode.Data.Structures;

public record Bytecode(List<BytecodeInstruction> Instructions)
{
    public override string ToString() => $"{string.Join("\n", Instructions)}";

    public int GetParametersCount()
    {
        Throw.AssertAlways(Instructions.Count != 0, "Cannot get parameters count: bytecode is empty");
        Throw.AssertAlways(Instructions[0].Type == InstructionType.MakeVariables,
            $"There is not {nameof(InstructionType.MakeVariables)} instruction: cannot get parameters");
        return Instructions[0].Arguments.Count;
    }
}