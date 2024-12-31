namespace ToMsilTranslator;

public static class RuntimeLibrary
{
    public static ToMsilTranslatorRuntimeData RuntimeData = null!;

    public static bool ToBool(TranslatorValue value) => value.IsTrue();
    public static TranslatorValue GetConst(int index) => RuntimeData.Constants[index];

    public static TranslatorValue CallFunc(string name) =>
        (TranslatorValue)RuntimeData.DynamicMethods[name].Invoke(null, [RuntimeData])!;

    public static void PushToStack(TranslatorValue value) => RuntimeData.IntermediateData.Push(value);
    public static TranslatorValue PopFromStack() => RuntimeData.IntermediateData.Pop();
}