namespace VirtualMachine.Vm.Data;

public class VmVariable(string name, AnyValueType varType)
{
    private AnyOpt _value;
    public string Name { get; } = name;
    public AnyValueType VarType { get; } = varType;

    public AnyOpt Value
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