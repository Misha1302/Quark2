using GrEmit;

namespace ToMsilTranslator;

public record FunctionCompileData(Dictionary<string, GroboIL.Local> Locals, Dictionary<string, GroboIL.Label> Labels);