namespace GenericBytecode.Instruction;

public static class InstructionManager
{
    private static int _curInstructionNumber;

    private static readonly Dictionary<InstructionValue, string> _instructionNames = [];

    // Build-in's
    // It's too hard to implement these instructions in plugins, so they should be supported by execution engine
    public static readonly InstructionValue Invalid = GetNextInstruction(nameof(Invalid));
    public static readonly InstructionValue Ret = GetNextInstruction(nameof(Ret));
    public static readonly InstructionValue JumpIfTrue = GetNextInstruction(nameof(JumpIfTrue));
    public static readonly InstructionValue SetLabel = GetNextInstruction(nameof(SetLabel));

    public static InstructionValue GetNextInstruction(string name)
    {
        var i = new InstructionValue(_curInstructionNumber++);
        _instructionNames[i] = name;
        return i;
    }

    public static string GetNameOfInstruction(InstructionValue instruction) => _instructionNames[instruction];
}