using System.Runtime.CompilerServices;

namespace TranslatorToMsil;

public static class RuntimeLibrary
{
    public static TranslatorRuntimeData RuntimeData = null!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ToBool(in AnyOpt value) => value.IsTrue();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AnyOpt GetConst(int index) => RuntimeData.Constants[index];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe AnyOpt CallFunc(string name) =>
        ((delegate*<TranslatorRuntimeData, AnyOpt>)RuntimeData.DynamicMethods[name])(RuntimeData);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PushToStack(in AnyOpt value) => RuntimeData.IntermediateData.Push(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AnyOpt PopFromStack() => RuntimeData.IntermediateData.Pop();
}