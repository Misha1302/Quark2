using CommonDataStructures;

namespace VirtualMachine.Vm.Preparing;

public static class SharpCallArgs
{
    /// <summary>
    ///     Makes list of arguments for operation CallSharp to call csharp functions
    /// </summary>
    /// <param name="func">static CSharp function to call</param>
    /// <returns>list of arguments</returns>
    public static List<VmValue> MakeCallSharpOperationArguments(Delegate func)
    {
        Throw.Assert(func.Method.ReturnType == typeof(void) || func.Method.ReturnType == typeof(Any));

        var parameters = func.Method.GetParameters();
        return
        [
            VmValue.Create(func.Method.MethodHandle.GetFunctionPointer(), NativeI64),
            VmValue.Create(parameters.Length, NativeI64),
            VmValue.Create(func.Method.ReturnType == typeof(Any) ? 1.0 : 0.0, Number),
            VmValue.Create(parameters.Length != 0 && parameters[0].ParameterType == typeof(IReadOnlyStack<Any>) ? 1.0 : 0.0,
                Number),
        ];
    }
}