namespace GenericBytecode2;

public static class TypesExtensions
{
    public static bool IsChildOrEq(this Type t1, Type t2) => t1.IsSubclassOf(t2) || t1 == t2;
}