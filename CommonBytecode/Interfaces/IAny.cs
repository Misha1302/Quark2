namespace CommonBytecode.Interfaces;

public interface IAny
{
    public object GetObjectValue();
    public BytecodeValueType GetAnyType();
}