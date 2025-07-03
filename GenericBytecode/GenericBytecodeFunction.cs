using ExceptionsManager;

namespace GenericBytecode;

public record GenericBytecodeFunction(string Name, FunctionBytecode Body)
{
    public override string ToString() => $"$$$ {Name}: \n{Body}";

    public List<Type> GetTypesStack(int instrIndex)
    {
        if (instrIndex <= -1) return [];

        var stack = GetTypesStack(instrIndex - 1);
        foreach (var action in Body.Instructions[instrIndex].Args)
        {
            VerifyStack(stack, action);

            var count = action.ParametersWithoutRefs.Length;
            stack.RemoveRange(stack.Count - count, count);
            stack.AddRange(action.ParametersRefs.Select(x => x.ParameterType.GetElementType()!));
        }

        return stack;
    }

    private void VerifyStack(List<Type> stack, InstructionAction action)
    {
        for (var i = 0; i < action.ParametersWithoutRefs.Length; i++)
        {
            var j = stack.Count - 1 - i;
            Throw.AssertAlways(action.Parameters[i].ParameterType == stack[j]);
        }
    }
}