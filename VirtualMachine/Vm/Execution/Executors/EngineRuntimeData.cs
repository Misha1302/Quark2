using CommonBytecode;

namespace VirtualMachine.Vm.Execution.Executors;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record EngineRuntimeData(
    VmModule Module,
    Action<VmOperation, int, VmFuncFrame, ExtendedStack<AnyOpt>>? LogAction,
    List<Interpreter> Interpreters,
    ExecutorConfiguration Configuration)
{
    public Interpreter CurInterpreter = null!;
}