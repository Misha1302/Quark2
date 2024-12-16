using AbstractExecutor;

namespace VirtualMachine.Vm.Execution.Executors;

public record EngineRuntimeData(
    VmModule Module,
    Action<VmOperation, int, VmFuncFrame, MyStack<VmValue>>? LogAction,
    List<Interpreter> Interpreters,
    ExecutorConfiguration Configuration)
{
    public Interpreter CurInterpreter = null!;
}