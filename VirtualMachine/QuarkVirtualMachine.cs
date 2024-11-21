using AbstractExecutor;
using CommonBytecode;
using VirtualMachine.Vm.Execution;

namespace VirtualMachine;

public class QuarkVirtualMachine : IExecutor
{
    public IEnumerable<Any> RunModule(BytecodeModule module)
    {
        var converter = new BytecodeConverter();
        var vmModule = converter.MakeVmModule(module);
        var engine = new Engine();
        var results = engine.Run(vmModule);
        return results.Select(x => x.ToAny());
    }
}