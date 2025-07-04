using AbstractExecutor;
using CommonLoggers;
using GenericBytecode;
using GenericBytecode.Interfaces;
using SharpAnyType;

namespace GenericBytecodeVirtualMachine;

public class GenericBytecodeVirtualMachine : IExecutor<GenericBytecodeConfiguration, IEnumerable<IBasicValue>>
{
    private ILogger _logger = null!;
    private OptimizedGenericBytecodeModule _module = null!;

    public IEnumerable<IBasicValue> RunModule() => RunFunction("Main", []);

    public IEnumerable<IBasicValue> RunFunction(string name, Span<Any> functionArguments)
    {
        var executor = new Interpreter(_logger);
        executor.AddFunction(new FunctionFrame(_module.Functions[name]));
        return executor.Run();
    }

    public void Init(GenericBytecodeConfiguration configuration)
    {
        _logger = configuration.Logger;
        _module = new OptimizedGenericBytecodeModule(configuration.Module);
    }
}