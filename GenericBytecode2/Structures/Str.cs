namespace GenericBytecode2.Structures;

public record Str(string Value) : IBasicValue
{
    public static implicit operator string(Str s) => s.Value;
}