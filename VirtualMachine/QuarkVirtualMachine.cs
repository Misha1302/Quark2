using AbstractExecutor;
using CommonBytecode;
using VirtualMachine.Vm.DataStructures;
using VirtualMachine.Vm.DataStructures.VmValues;
using VirtualMachine.Vm.Execution;
using VirtualMachine.Vm.Operations;

namespace VirtualMachine;

public class QuarkVirtualMachine : IExecutor
{
    public IEnumerable<Any> RunModule(BytecodeModule module, object?[] arguments)
    {
        var logAction = (Action<Operation, int, VmFuncFrame, MyStack<VmValue>>?)arguments[0];

        var converter = new BytecodeConverter();
        var vmModule = converter.MakeVmModule(module);
        var engine = new Engine();
        var results = engine.Run(vmModule, logAction);
        return results.Select(x => x.ToAny());
    }
}