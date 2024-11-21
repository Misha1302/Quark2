using VirtualMachine.Vm.Operations;

namespace VirtualMachine.Vm.DataStructures;

public record VmFuncFrame(string Name, List<Operation> Ops, int Ip, List<VmVariable> Variables, List<Label> Labels)
{
    public int Ip = Ip;

    public VmFuncFrame(VmFunction func) : this(func.Name, func.Ops, 0, [], func.Labels)
    {
        foreach (var variable in func.Variables)
            Variables.Add(new VmVariable(variable.Name, variable.VarType));
    }
}