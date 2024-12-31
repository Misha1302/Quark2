namespace ToMsilTranslator;

public record ToMsilTranslatorRuntimeData(
    List<TranslatorValue> Constants,
    Dictionary<string, DynamicMethod> DynamicMethods,
    Stack<TranslatorValue> IntermediateData,
    BytecodeModule Module
);