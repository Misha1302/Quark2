namespace GenericBytecode.Instruction;

public readonly record struct InstructionValue(int Value)
{
    public override string ToString() => InstructionManager.GetNameOfInstruction(this);
}