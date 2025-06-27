namespace GenericBytecode2;

public readonly record struct InstructionValue(int Value)
{
    // Build-in's
    public static readonly InstructionValue Invalid = InstructionManager.GetNextInstruction(nameof(Invalid));
    public static readonly InstructionValue Ret = InstructionManager.GetNextInstruction(nameof(Ret));
    public static readonly InstructionValue Goto = InstructionManager.GetNextInstruction(nameof(Goto));

    public override string ToString() => InstructionManager.GetNameOfInstruction(this);
}