using AbstractExecutor;
using CommonBytecode.Data.Structures;
using VirtualMachine.Vm.Execution;
using VirtualMachine.Vm.Execution.Executors;
using VirtualMachine.Vm.Preparing;

namespace VirtualMachine;

public class QuarkVirtualMachine(Dictionary<string, Action<EngineRuntimeData>> getBuildInFunctions) : IExecutor
{
    public IEnumerable<Any> RunModule(BytecodeModule module, object?[] arguments)
    {
        var logAction = (Action<VmOperation, int, VmFuncFrame, MyStack<VmValue>>?)arguments[0];

        var converter = new BytecodeConverter();
        var vmModule = converter.MakeVmModule(module);
        var engine = new Engine();
        var results = engine.Run(vmModule, logAction);
        return results.Select(x => x.ToAny());
    }
}