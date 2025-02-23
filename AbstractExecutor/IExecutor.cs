using CommonBytecode.Data.AnyValue;

namespace AbstractExecutor;

public interface IExecutor
{
    /// <summary>
    ///     Initialize executor to run with current configuration
    /// </summary>
    /// <param name="configuration">Data to initialize</param>
    void Init(ExecutorConfiguration configuration);

    /// <summary>
    ///     Executes a given bytecode module and returns the result as an enumeration of Any values.
    /// </summary>
    /// <returns>An enumeration of Any values representing the execution results.</returns>
    IEnumerable<Any> RunModule();

    /// <summary>
    ///     Executes a named function within a given bytecode module with specified arguments and returns the result as an
    ///     enumeration of Any values.
    /// </summary>
    /// <param name="name">The name of the function to execute.</param>
    /// <param name="functionArguments">A span of Any values representing the arguments for the function.</param>
    /// <returns>An enumeration of Any values representing the execution results.</returns>
    IEnumerable<Any> RunFunction(string name, Span<Any> functionArguments);
}