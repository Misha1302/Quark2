namespace GenericBytecode;

public readonly record struct InstructionValue(int Value)
{
    // Build-in's
    public static readonly InstructionValue Invalid = InstructionManager.GetNextInstruction(nameof(Invalid));
    public static readonly InstructionValue Ret = InstructionManager.GetNextInstruction(nameof(Ret));
    public static readonly InstructionValue JumpIfTrue = InstructionManager.GetNextInstruction(nameof(JumpIfTrue));
    public static readonly InstructionValue SetLabel = InstructionManager.GetNextInstruction(nameof(SetLabel));

    public override string ToString() => InstructionManager.GetNameOfInstruction(this);

    public bool IsSomeOf(params InstructionValue[] instructions)
    {
        var thisLoc = this;
        return instructions.Any(x => x == thisLoc);
    }
}