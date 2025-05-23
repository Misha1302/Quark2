namespace GenericBytecode2;

public record GenericBytecode(List<InstructionValue> Instructions)
{
    public override string ToString() => $"{string.Join("\n", Instructions)}";
}