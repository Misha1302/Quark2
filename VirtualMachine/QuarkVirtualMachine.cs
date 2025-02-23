using AbstractExecutor;
using CommonBytecode.Data.Structures;
using VirtualMachine.Vm.Execution.Executors;
using VirtualMachine.Vm.Preparing;

namespace VirtualMachine;

public class QuarkVirtualMachine(ExecutorConfiguration configuration) : IExecutor
{
    private Engine _engine = null!;

    public IEnumerable<Any> RunModule(BytecodeModule module) => RunFunction(module, "Main", []);

    public IEnumerable<Any> RunFunction(BytecodeModule module, string name, Span<Any> functionArguments)
    {
        var vmModule = InitIfNeed(module);
        var results = _engine.RunFunction(vmModule, name, functionArguments);
        return results.Select(x => x.ToAny());
    }

    private VmModule InitIfNeed(BytecodeModule module)
    {
        var converter = new BytecodeConverter();
        var vmModule = converter.MakeVmModule(module);

        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
        _engine ??= new Engine(configuration);
        return vmModule;
    }

    public void PrepareToExecute(BytecodeModule module)
    {
        InitIfNeed(module);
    }
}