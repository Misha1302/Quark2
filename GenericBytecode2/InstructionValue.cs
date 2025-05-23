namespace GenericBytecode2;

public struct InstructionValue(int value)
{
    public readonly int Value = value;

    public static readonly InstructionValue Invalid = new(0);
}