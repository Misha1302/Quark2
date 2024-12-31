using System.Reflection.Emit;
using CommonBytecode.Data.AnyValue;

namespace ToMsilTranslator;

public record ToMsilTranslatorRuntimeData(
    List<Any> Constants,
    Dictionary<string, DynamicMethod> DynamicMethods,
    Stack<Any> IntermediateData
);