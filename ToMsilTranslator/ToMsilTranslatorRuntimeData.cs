using System.Reflection.Emit;
using CommonBytecode.Data.AnyValue;

namespace ToMsilTranslator;

public record ToMsilTranslatorRuntimeData(
    List<TranslatorValue> Constants,
    Dictionary<string, DynamicMethod> DynamicMethods,
    Stack<TranslatorValue> IntermediateData
);