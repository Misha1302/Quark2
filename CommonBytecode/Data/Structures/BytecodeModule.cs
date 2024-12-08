namespace CommonBytecode.Data.Structures;

public record BytecodeModule(string Name, List<BytecodeFunction> Functions)
{
    public override string ToString() => $"{Name}; \n[\n{string.Join("\n", Functions)}\n]";
}