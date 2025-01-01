using AbstractExecutor;
using CommonBytecode.Data.Structures;
using CommonDataStructures;
using VirtualMachine.Vm.Execution;
using VirtualMachine.Vm.Execution.Executors;
using VirtualMachine.Vm.Preparing;

namespace VirtualMachine;

public class QuarkVirtualMachine(ExecutorConfiguration configuration) : IExecutor
{
    private Engine _engine = null!;

    public IEnumerable<Any> RunModule(BytecodeModule module, object?[] arguments)
    {
        var logAction = Init(module, arguments, out var vmModule);
        var results = _engine.Run(vmModule, logAction);
        return results.Select(x => x.ToAny());
    }

    public IEnumerable<Any> RunFunction(BytecodeModule module, string name, Span<Any> arguments)
    {
        var args = arguments.ToArray().Select(x => x.MakeAnyOptList()[0]).ToArray();
        var results = _engine.EngineRuntimeData.CurInterpreter.ExecuteFunction(name, args, _engine.EngineRuntimeData);
        return [results.ToAny()];
    }

    private Action<VmOperation, int, VmFuncFrame, MyStack<AnyOpt>>? Init(BytecodeModule module, object?[] arguments,
        out VmModule vmModule)
    {
        var logAction = (Action<VmOperation, int, VmFuncFrame, MyStack<AnyOpt>>?)arguments[0];

        var converter = new BytecodeConverter();
        vmModule = converter.MakeVmModule(module);
        _engine = new Engine(configuration);
        return logAction;
    }
}