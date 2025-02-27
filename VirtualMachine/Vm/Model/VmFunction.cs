namespace VirtualMachine.Vm.Model;

public record VmFunction(List<VmOperation> Ops, string Name, List<VmVariable> Variables, List<VmLabel> Labels);