namespace GenericBytecode2;

public record GenericBytecodeFunction(string Name, FunctionBytecode Body)
{
    public override string ToString() => $"$$$ {Name}: \n{Body}";
}