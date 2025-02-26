namespace SharpAnyType;

public interface IAny
{
    public Any ToAny() => new(GetObjectValue(), GetAnyType());

    public object GetObjectValue();
    public AnyValueType GetAnyType();
}