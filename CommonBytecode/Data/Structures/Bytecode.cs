using CommonBytecode.Enums;
using ExceptionsManager;

namespace CommonBytecode.Data.Structures;

public record Bytecode(List<BytecodeInstruction> Instructions)
{
    public override string ToString() => $"{string.Join("\n", Instructions)}";

    public int GetParametersCount()
    {
        Throw.Assert(Instructions.Count != 0);
        Throw.Assert(Instructions[0].Type == InstructionType.MakeVariables);
        return Instructions[0].Arguments.Count;
    }
}