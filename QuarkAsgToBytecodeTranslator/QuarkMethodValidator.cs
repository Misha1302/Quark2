using System.Reflection;
using CommonDataStructures;

namespace AsgToBytecodeTranslator;

public class QuarkMethodValidator : IMethodValidator
{
    public bool IsValidMethod(MethodInfo method) =>
        IsValidModifiers(method) && IsValidParameters(method.GetParameters()) && IsValidReturn(method.ReturnType);

    private static bool IsValidModifiers(MethodInfo method) =>
        method is { IsStatic: true, IsPublic: true, IsGenericMethod: false };

    private static bool IsValidReturn(Type type) =>
        type == typeof(void) ||
        type == typeof(Any) ||
        type == typeof(IAny) ||
        type.IsSubclassOf(typeof(IAny));

    private static bool IsValidParameters(ParameterInfo[] parameters)
    {
        var a = parameters.Length == 1 && parameters[0].ParameterType.IsSubclassOf(typeof(IReadOnlyStack<IAny>));
        var b = parameters.All(x =>
            x.ParameterType == typeof(IAny) || x.ParameterType == typeof(Any) ||
            x.ParameterType.IsSubclassOf(typeof(IAny)));
        return a || b;
    }
}