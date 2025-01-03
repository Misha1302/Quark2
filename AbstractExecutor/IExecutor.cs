using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;

namespace AbstractExecutor;

public interface IExecutor
{
    /// <summary>
    ///     Executes a given bytecode module and returns the result as an enumeration of Any values.
    /// </summary>
    /// <param name="module">The bytecode module to execute.</param>
    /// <returns>An enumeration of Any values representing the execution results.</returns>
    IEnumerable<Any> RunModule(BytecodeModule module);

    /// <summary>
    ///     Executes a named function within a given bytecode module with specified arguments and returns the result as an
    ///     enumeration of Any values.
    /// </summary>
    /// <param name="module">The bytecode module containing the function to execute.</param>
    /// <param name="name">The name of the function to execute.</param>
    /// <param name="functionArguments">A span of Any values representing the arguments for the function.</param>
    /// <returns>An enumeration of Any values representing the execution results.</returns>
    IEnumerable<Any> RunFunction(BytecodeModule module, string name, Span<Any> functionArguments);
}