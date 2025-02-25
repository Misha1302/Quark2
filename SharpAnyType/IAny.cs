namespace SharpAnyType;

public interface IAny
{
    public object GetObjectValue();
    public BytecodeValueType GetAnyType();
}