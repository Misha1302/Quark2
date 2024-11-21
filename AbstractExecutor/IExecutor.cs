using CommonBytecode;

namespace AbstractExecutor;

public interface IExecutor
{
    IEnumerable<Any> RunModule(BytecodeModule module);
}