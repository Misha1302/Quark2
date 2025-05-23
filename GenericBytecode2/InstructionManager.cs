namespace GenericBytecode2;

public static class InstructionManager
{
    private static int _curInstructionNumber = 1;

    public static InstructionValue GetNextInstruction() => new(_curInstructionNumber++);
}