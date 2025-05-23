namespace GenericBytecode2;

public record InstructionAction(Action Act, Action<string>? DelegateToString = null);