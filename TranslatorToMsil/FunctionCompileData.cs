namespace TranslatorToMsil;

public record FunctionCompileData(Dictionary<string, GroboIL.Local> Locals, Dictionary<string, GroboIL.Label> Labels);