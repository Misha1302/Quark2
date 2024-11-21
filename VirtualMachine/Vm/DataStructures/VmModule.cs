namespace VirtualMachine.Vm.DataStructures;

public record VmModule(List<VmFunction> Functions)
{
    public VmFunction this[string funcName] => Functions.First(x => x.Name == funcName);
}