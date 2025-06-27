namespace GenericBytecode2;

public record Instruction(InstructionValue Value, InstructionAction[] Args)
{
    public override string ToString() => $"{Value}: [{string.Join<InstructionAction>(", ", Args)}]";
}