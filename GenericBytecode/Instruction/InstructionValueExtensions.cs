namespace GenericBytecode.Instruction;

public static class InstructionValueExtensions
{
    public static bool IsSomeOf(this InstructionValue value, params InstructionValue[] instructions) =>
        instructions.Any(x => x == value);
}