namespace VirtualMachine.Vm.Data;

public class VmVariable(string name, BytecodeValueType varType)
{
    private VmValue _value;
    public string Name { get; } = name;
    public BytecodeValueType VarType { get; } = varType;

    public VmValue Value
    {
        get => _value;
        set
        {
            if ((value.Type & VarType) == 0) Throw.InvalidOpEx();
            _value = value;
        }
    }

    public override string ToString() => this.ToStringExtension();
}