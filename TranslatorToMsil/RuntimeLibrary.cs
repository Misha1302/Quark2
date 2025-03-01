namespace TranslatorToMsil;

public static class RuntimeLibrary
{
    public static TranslatorRuntimeData RuntimeData = null!;

    public static bool ToBool(in AnyOpt value) => value.IsTrue();

    public static AnyOpt GetConst(int index) => RuntimeData.Constants[index];

    public static unsafe AnyOpt CallFunc(string name) =>
        ((delegate*<TranslatorRuntimeData, AnyOpt>)RuntimeData.DynamicMethods[name])(RuntimeData);

    public static void PushToStack(in AnyOpt value) => RuntimeData.IntermediateData.Push(value);

    public static AnyOpt PopFromStack() => RuntimeData.IntermediateData.Pop();
}