using SharpAnyType;

namespace QuarkStructures;

public record QuarkStructure(string Name, Dictionary<string, Any> Fields);