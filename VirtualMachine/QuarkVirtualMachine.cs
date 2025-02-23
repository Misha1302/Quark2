using AbstractExecutor;
using CommonBytecode.Data.Structures;
using VirtualMachine.Vm.Execution.Executors;
using VirtualMachine.Vm.Preparing;

namespace VirtualMachine;

public class QuarkVirtualMachine(ExecutorConfiguration configuration) : IExecutor
{
    private Engine _engine = null!;
    private VmModule _vmModule = null!;

    public IEnumerable<Any> RunModule() => RunFunction("Main", []);

    public IEnumerable<Any> RunFunction(string name, Span<Any> functionArguments)
    {
        Throw.AssertAlways(_vmModule != null, "VM module was not initialized");
        var results = _engine.RunFunction(_vmModule, name, functionArguments);
        return results.Select(x => x.ToAny());
    }

    public void PrepareToRun(BytecodeModule module)
    {
        _vmModule = Init(module);
    }

    private VmModule Init(BytecodeModule module)
    {
        var converter = new BytecodeConverter();
        var vmModule = converter.MakeVmModule(module);

        _engine = new Engine(configuration);
        return vmModule;
    }
}