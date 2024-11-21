using VirtualMachine.Vm.DataStructures;
using VirtualMachine.Vm.DataStructures.VmValues;
using VirtualMachine.Vm.Operations;

namespace VirtualMachine.Vm.Execution;

public record EngineRuntimeData(
    VmModule Module,
    Action<Operation, int, VmFuncFrame, MyStack<VmValue>>? LogAction,
    List<Interpreter> Interpreters);