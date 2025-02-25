namespace CommonBytecode.Enums;

public enum InstructionType
{
    // ReSharper disable once UnusedMember.Global
    Invalid,
    PushConst,
    MathOrLogicOp,
    LoadLocal,
    SetLocal,
    BrOp,
    CallSharp,
    CallFunc,
    Ret,
    Label,
    Drop,
    MakeVariables,
}