namespace CommonBytecode.Data.Structures;

public record BytecodeFunction(string Name, Bytecode Code)
{
    public override string ToString() => $"{Name} \n{{\n{Code}\n}}";
}