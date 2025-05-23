using CommonBytecode;

namespace VirtualMachine;

public class QuarkVirtualMachine : IExecutor<ExecutorConfiguration>
{
    private Engine _engine = null!;
    private VmModule _vmModule = null!;

    public void Init(ExecutorConfiguration configuration)
    {
        var converter = new BytecodeConverter();
        _vmModule = converter.MakeVmModule(configuration.Module);
        _engine = new Engine(configuration);
    }

    public IEnumerable<Any> RunModule() => RunFunction("Main", []);

    public IEnumerable<Any> RunFunction(string name, Span<Any> functionArguments)
    {
        Throw.AssertAlways(_vmModule != null, "VM module was not initialized");
        var results = _engine.RunFunction(_vmModule, name, functionArguments);
        return results.Select(x => x.ToAny());
    }
}