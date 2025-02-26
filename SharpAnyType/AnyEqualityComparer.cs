namespace SharpAnyType;

public class AnyEqualityComparer : IEqualityComparer<Any>
{
    public static readonly AnyEqualityComparer Instance = new();


    public bool Equals(Any x, Any y) => EqualsCustom(x, y);

    public int GetHashCode(Any obj) => HashCode.Combine(obj.Type, obj.Value);

    private bool EqualsCustom(Any x, Any y)
    {
        if (x.GetType() != y.GetType()) return false;

        if (x.Type == Nil && y.Type == Nil) return true;
        if (x.Type == Nil) return false;
        if (y.Type == Nil) return false;

        return x.Type == y.Type && x.Value.Equals(y.Value);
    }
}