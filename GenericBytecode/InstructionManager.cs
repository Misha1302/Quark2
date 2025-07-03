namespace GenericBytecode;

public static class InstructionManager
{
    private static int _curInstructionNumber;

    private static readonly Dictionary<InstructionValue, string> _instructionNames = [];

    public static InstructionValue GetNextInstruction(string name)
    {
        var i = new InstructionValue(_curInstructionNumber++);
        _instructionNames[i] = name;
        return i;
    }

    public static string GetNameOfInstruction(InstructionValue instruction) => _instructionNames[instruction];
}