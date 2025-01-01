using CommonDataStructures;

namespace ToMsilTranslator;

public record ToMsilTranslatorRuntimeData(
    List<AnyOpt> Constants,
    Dictionary<string, DynamicMethod> DynamicMethods,
    Stack<AnyOpt> IntermediateData,
    BytecodeModule Module
);