using SharpAnyType;

namespace QuarkStructuresLibrary;

public record QuarkStructure(string Name, Dictionary<string, Any> Fields)
{
    public override string ToString() => $"{Name}: [{string.Join(", ", Fields.Select(x => $"{x.Key}: {x.Value}"))}]";
}