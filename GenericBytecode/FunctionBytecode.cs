namespace GenericBytecode;

public record FunctionBytecode(List<Instruction.Instruction> Instructions)
{
    public override string ToString() => $"{string.Join("\n", Instructions)}";
}