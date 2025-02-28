using CommonDataStructures;
using SharpAnyType;

namespace SharpLibrariesImporter;

public static class MethodValidator
{
    public static bool IsValidMethod(MethodInfo method) =>
        IsValidModifiers(method) && IsValidParameters(method.GetParameters()) && IsValidReturn(method.ReturnType);

    private static bool IsValidModifiers(MethodInfo method) =>
        method is { IsStatic: true, IsPublic: true, IsGenericMethod: false };

    private static bool IsValidReturn(Type methodReturnType) =>
        methodReturnType == typeof(void) ||
        methodReturnType == typeof(Any) ||
        methodReturnType == typeof(IAny) ||
        methodReturnType.IsSubclassOf(typeof(IAny));

    private static bool IsValidParameters(ParameterInfo[] parameters)
    {
        var a = parameters.Length == 1 && parameters[0].ParameterType.IsSubclassOf(typeof(IReadOnlyStack<IAny>));
        var b = parameters.All(x =>
            x.ParameterType == typeof(IAny) || x.ParameterType == typeof(Any) ||
            x.ParameterType.IsSubclassOf(typeof(IAny)));
        return a || b;
    }
}