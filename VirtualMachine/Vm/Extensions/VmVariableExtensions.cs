namespace VirtualMachine.Vm.Extensions;

public static class VmVariableExtensions
{
    public static string ToStringExtension(this VmVariable variable) =>
        $"{variable.Name}: {variable.Value} ({variable.VarType})";
}