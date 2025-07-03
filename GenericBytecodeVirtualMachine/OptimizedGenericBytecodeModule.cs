using GenericBytecode;

namespace GenericBytecodeVirtualMachine;

public class OptimizedGenericBytecodeModule
{
    public readonly Dictionary<string, GenericBytecodeFunction> Functions;

    public OptimizedGenericBytecodeModule(GenericBytecodeModule configuration)
    {
        Functions = configuration.Functions.ToDictionary(x => x.Name, x => x);
    }
}