using VirtualMachine.Vm.Operations;

namespace VirtualMachine.Vm.DataStructures;

public record VmFunction(List<Operation> Ops, string Name, List<VmVariable> Variables, List<Label> Labels);