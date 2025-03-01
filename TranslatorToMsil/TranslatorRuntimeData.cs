namespace TranslatorToMsil;

public record TranslatorRuntimeData(
    AnyOpt[] Constants,
    FrozenDictionary<string, nint> DynamicMethods,
    OptimizedStack<AnyOpt> IntermediateData
);