using System.Reflection;
using System.Reflection.Emit;

namespace RuntimeHackes;

public static class RuntimeHacks
{
    private static readonly MethodInfo _getMethodDescriptor =
        typeof(DynamicMethod).GetMethod("GetMethodDescriptor", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static nint GetNativePointer(this DynamicMethod method)
    {
        var handleValue = (RuntimeMethodHandle)_getMethodDescriptor.Invoke(method, null)!;
        handleValue.ForbidToCollectIt();
        return handleValue.GetFunctionPointer();
    }
}