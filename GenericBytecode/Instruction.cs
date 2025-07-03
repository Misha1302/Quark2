namespace GenericBytecode;

public record Instruction(InstructionValue Value, ArgsCollection Args)
{
    public override string ToString() => $"{Value}: [{string.Join(", ", Args)}]";
}