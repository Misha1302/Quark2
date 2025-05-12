namespace VirtualMachine.Vm.Preparing;

public static class SharpCallArgs
{
    /// <summary>
    ///     Makes list of arguments for operation CallSharp to call csharp functions
    /// </summary>
    /// <param name="func">static CSharp function to call</param>
    /// <returns>list of arguments</returns>
    public static List<AnyOpt> MakeCallSharpOperationArguments(Delegate func)
    {
        Throw.AssertAlways(func.Method.ReturnType == typeof(void) || func.Method.ReturnType == typeof(Any),
            "Func must return void or Any");

        RuntimeHelpers.PrepareDelegate(func);

        var parameters = func.Method.GetParameters();
        return
        [
            AnyOpt.Create(func.Method.MethodHandle.GetFunctionPointer(), NativeI64),
            AnyOpt.Create(parameters.Length, NativeI64),
            AnyOpt.Create(func.Method.ReturnType == typeof(Any) ? 1.0 : 0.0, Number),
            AnyOpt.Create(
                parameters.Length != 0 && parameters[0].ParameterType == typeof(IReadOnlyStack<Any>) ? 1.0 : 0.0,
                Number),
        ];
    }
}