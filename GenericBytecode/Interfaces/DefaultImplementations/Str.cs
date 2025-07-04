namespace GenericBytecode.Interfaces.DefaultImplementations;

public record Str(string Value) : IBasicValue, IStr
{
    public string GetString() => Value;
}