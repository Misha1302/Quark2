using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;

namespace AbstractExecutor;

public interface IExecutor
{
    IEnumerable<Any> RunModule(BytecodeModule module, object?[] arguments);
}