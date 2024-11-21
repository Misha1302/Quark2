namespace CommonBytecode;

public interface IAny
{
    public object GetObjectValue();
    public BytecodeValueType GetAnyType();
}