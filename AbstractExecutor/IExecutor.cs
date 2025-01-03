using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;

namespace AbstractExecutor;

public interface IExecutor
{
    /// <summary>
    /// Executes a bytecode module with the provided arguments and returns the result of execution.
    /// The method takes in a BytecodeModule instance and an array of optional objects as arguments,
    /// then runs the module's code and produces a sequence of Any values representing the output.
    /// </summary>
    IEnumerable<Any> RunModule(BytecodeModule module);

    /// <summary>
    /// Executes a function within a bytecode module by its specified name using the given arguments.
    /// This method locates the named function inside the provided BytecodeModule, executes it with
    /// the supplied arguments encapsulated in a Span of Any instances, and returns the resulting
    /// sequence of Any values produced by the function.
    /// </summary>
    IEnumerable<Any> RunFunction(BytecodeModule module, string name, Span<Any> functionArguments);
}