namespace AsgToBytecodeTranslator;

public class QuarkMethodValidator : IMethodValidator
{
    public bool IsValidMethod(MethodInfo method) =>
        IsValidModifiers(method) && IsValidParameters(method.GetParameters()) && IsValidReturn(method.ReturnType);

    private static bool IsValidModifiers(MethodInfo method) =>
        method is { IsStatic: true, IsPublic: true, IsGenericMethod: false };

    private static bool IsValidReturn(Type type) => type == typeof(void) || type == typeof(Any);

    private static bool IsValidParameters(ParameterInfo[] parameters)
    {
        var a = parameters.Length == 1 && parameters[0].ParameterType == typeof(IReadOnlyStack<Any>);
        var b = parameters.All(x => x.ParameterType == typeof(Any));
        return a || b;
    }
}