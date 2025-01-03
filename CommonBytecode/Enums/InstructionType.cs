namespace CommonBytecode.Enums;

public enum InstructionType
{
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