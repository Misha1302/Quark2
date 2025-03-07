using SharpAnyType;

namespace QuarkStructuresLibrary;

public class QuarkStructuresLibrary
{
    public static readonly QuarkStructuresLibrary Instance = new();

    public List<QuarkStructure> Structures { get; } = [];

    public void AddStructure(QuarkStructure s)
    {
        Structures.Add(s);
    }

    public static Any GetField(Any structure, Any fieldName) =>
        structure.Get<QuarkStructure>().Fields[fieldName.Get<string>()];

    public static void SetField(Any value, Any structure, Any fieldName) =>
        structure.Get<QuarkStructure>().Fields[fieldName.Get<string>()] = value;

    public static Any CreateStruct(Any structName) =>
        new(Instance.Structures.First(x => x.Name == structName.Get<string>()), AnyValueType.SomeSharpObject);
}