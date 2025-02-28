using System.Collections.Frozen;
using OptimizedStack;

namespace TranslatorToMsil;

public record TranslatorRuntimeData(
    List<AnyOpt> Constants,
    FrozenDictionary<string, nint> DynamicMethods,
    OptimizedStack<AnyOpt> IntermediateData
);