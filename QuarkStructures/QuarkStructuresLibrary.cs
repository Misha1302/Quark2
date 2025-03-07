using SharpAnyType;

namespace QuarkStructures;

public class QuarkStructuresLibrary
{
    public static readonly QuarkStructuresLibrary Instance = new();

    public List<QuarkStructure> Structures { get; } = [];

    public void AddStructure(QuarkStructure s)
    {
        Structures.Add(s);
    }

    public static Any GetField(Any structure, string fieldName) =>
        structure.Get<QuarkStructure>().Fields[fieldName];

    public static void SetField(Any value, Any structure, string fieldName) =>
        structure.Get<QuarkStructure>().Fields[fieldName] = value;

    public static Any CreateStruct(Any structName) =>
        new(Instance.Structures.First(x => x.Name == structName.Get<string>()), AnyValueType.SomeSharpObject);
}