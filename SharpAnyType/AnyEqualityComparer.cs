namespace SharpAnyType;

public class AnyEqualityComparer : IEqualityComparer<Any>
{
    public static readonly AnyEqualityComparer Instance = new();


    public bool Equals(Any x, Any y) => x.EqualExt(y);

    public int GetHashCode(Any obj) => HashCode.Combine(obj.Type, obj.Value);
}