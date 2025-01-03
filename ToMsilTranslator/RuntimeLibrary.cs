using CommonDataStructures;

namespace ToMsilTranslator;

public static class RuntimeLibrary
{
    public static ToMsilTranslatorRuntimeData RuntimeData = null!;

    public static bool ToBool(AnyOpt value) => value.IsTrue();

    public static AnyOpt GetConst(int index) => RuntimeData.Constants[index];

    public static AnyOpt CallFunc(string name) =>
        (AnyOpt)RuntimeData.DynamicMethods[name].Invoke(null, [RuntimeData])!;

    public static void PushToStack(AnyOpt value) => RuntimeData.IntermediateData.Push(value);

    public static AnyOpt PopFromStack() => RuntimeData.IntermediateData.Pop();
}