using CommonExtensions;
using SharpAnyType;

namespace QuarkStructuresLibrary;

public class QuarkStructuresLibrary
{
    public static readonly QuarkStructuresLibrary Instance = new();

    public Dictionary<string, QuarkStructure> Structures { get; } = [];

    public void AddStructure(QuarkStructure s)
    {
        Structures.CreateOrAdd(s.Name, s);
        Structures[s.Name] = s;
    }

    public static Any GetField(Any structure, Any fieldName) =>
        structure.Get<QuarkStructure>().Fields[fieldName.Get<string>()];

    public static void SetField(Any value, Any structure, Any fieldName) =>
        structure.Get<QuarkStructure>().Fields[fieldName.Get<string>()] = value;

    public static Any CreateStruct(Any structName) =>
        new(Clone(Instance.Structures[structName.Get<string>()]), AnyValueType.SomeSharpObject);

    private static QuarkStructure Clone(QuarkStructure instanceStructure) =>
        instanceStructure with { Fields = new Dictionary<string, Any>(instanceStructure.Fields) };
}