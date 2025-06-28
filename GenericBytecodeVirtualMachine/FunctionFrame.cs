using GenericBytecode2;
using GenericBytecode2.Structures;

namespace GenericBytecodeVirtualMachine;

public class FunctionFrame(GenericBytecodeFunction bytecode)
{
    private readonly Lazy<Dictionary<string, int>> _lazyLabels = new(() =>
        bytecode.Body.Instructions
            .Where(x => x.Value == InstructionValue.SetLabel)
            .Select((x, i) => (name: x.Args[0].Invoke<Str>().Value, ind: i))
            .ToDictionary(x => x.name, x => x.ind)
    );

    public readonly GenericBytecodeFunction Bytecode = bytecode;

    public int Sp;

    public Dictionary<string, int> Labels => _lazyLabels.Value;
}