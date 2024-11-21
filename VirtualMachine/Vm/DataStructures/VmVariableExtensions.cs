namespace VirtualMachine.Vm.DataStructures;

public static class VmVariableExtensions
{
    public static string ToStringExtension(this VmVariable variable) =>
        $"{variable.Name}: {variable.Value} ({variable.VarType})";
}