namespace GenericBytecode2;

public record FunctionBytecode(List<Instruction> Instructions)
{
    public override string ToString() => $"{string.Join("\n", Instructions)}";
}