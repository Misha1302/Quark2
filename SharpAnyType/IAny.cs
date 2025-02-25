namespace SharpAnyType;

public interface IAny
{
    public object GetObjectValue();
    public AnyValueType GetAnyType();
}