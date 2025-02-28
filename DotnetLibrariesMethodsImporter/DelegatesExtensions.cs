namespace SharpLibrariesImporter;

public static class DelegatesExtensions
{
    public static Delegate CreateDelegateCustom(this MethodInfo methodInfo, object? target)
    {
        Func<Type[], Type> getType;
        var isAction = methodInfo.ReturnType == typeof(void);
        var types = methodInfo.GetParameters().Select(p => p.ParameterType);

        if (isAction)
        {
            getType = Expression.GetActionType;
        }
        else
        {
            getType = Expression.GetFuncType;
            types = types.Concat([methodInfo.ReturnType]);
        }

        return methodInfo.IsStatic
            ? Delegate.CreateDelegate(getType(types.ToArray()), methodInfo)
            : Delegate.CreateDelegate(getType(types.ToArray()), target ?? Throw.InvalidOpEx<object>(), methodInfo.Name);
    }
}