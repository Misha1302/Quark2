using SharpAnyType;

namespace QuarkTypesConverter;

public static class TypesConverter
{
    public static Any ToStr(Any any) => any.ToString();
}