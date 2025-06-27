using AbstractExecutor;
using GenericBytecode2;
using SharpAnyType;

namespace GenericBytecodeVirtualMachine;

public class GenericBytecodeVirtualMachine : IExecutor<GenericBytecodeModule, IEnumerable<IBasicValue>>
{
    private OptimizedGenericBytecodeModule _module = null!;

    public void Init(GenericBytecodeModule configuration)
    {
        _module = new OptimizedGenericBytecodeModule(configuration);
    }

    public IEnumerable<IBasicValue> RunModule() => RunFunction("Main", []);

    public IEnumerable<IBasicValue> RunFunction(string name, Span<Any> functionArguments)
    {
        var executor = new Interpreter();
        executor.AddFunction(new FunctionFrame(_module.Functions[name]));
        return executor.Run();
    }
}