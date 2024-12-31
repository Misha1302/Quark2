using CommonBytecode.Data.AnyValue;

namespace ToMsilTranslator;

public static class RuntimeLibrary
{
    public static ToMsilTranslatorRuntimeData RuntimeData = null!;

    public static bool ToBool(Any value) => value.IsTrue();
    public static Any GetConst(int index) => RuntimeData.Constants[index];

    public static Any CallFunc(string name) => (Any)RuntimeData.DynamicMethods[name].Invoke(null, [RuntimeData])!;

    public static void PushToStack(Any value) => RuntimeData.IntermediateData.Push(value);
    public static Any PopFromStack() => RuntimeData.IntermediateData.Pop();
}