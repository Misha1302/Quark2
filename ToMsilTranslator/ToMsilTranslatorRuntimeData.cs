using System.Collections.Frozen;
using OptimizedStack;

namespace ToMsilTranslator;

public record ToMsilTranslatorRuntimeData(
    List<AnyOpt> Constants,
    FrozenDictionary<string, nint> DynamicMethods,
    OptimizedStack<AnyOpt> IntermediateData
);