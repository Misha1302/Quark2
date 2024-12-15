using CommonDataStructures;

namespace VirtualMachine.Vm.Execution;

public class StackOfAnies(IReadOnlyStack<VmValue> stack) : IReadOnlyStack<Any>
{
    public Any Get(int ind) => stack.Get(ind).ToAny();
}