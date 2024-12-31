namespace VirtualMachine.Vm.Data;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record VmFuncFrame(string Name, List<VmOperation> Ops, int Ip, List<VmVariable> Variables, List<Label> Labels)
{
    public int Ip = Ip;

    public VmFuncFrame(VmFunction func) : this(func.Name, func.Ops, 0, [], func.Labels)
    {
        foreach (var variable in func.Variables)
            Variables.Add(new VmVariable(variable.Name, variable.VarType));
    }
}