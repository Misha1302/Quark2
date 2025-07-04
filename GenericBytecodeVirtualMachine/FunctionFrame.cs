using GenericBytecode;
using GenericBytecode.Instruction;
using GenericBytecode.Interfaces;

namespace GenericBytecodeVirtualMachine;

public class FunctionFrame(GenericBytecodeFunction bytecode)
{
    private readonly Lazy<Dictionary<string, int>> _lazyLabels = new(() =>
        bytecode.Body.Instructions
            .Where(x => x.Value == InstructionManager.SetLabel)
            .Select((x, i) => (name: x.Args[0].Invoke<IStr>().GetString(), ind: i))
            .ToDictionary(x => x.name, x => x.ind)
    );

    public readonly GenericBytecodeFunction Bytecode = bytecode;

    public int Sp;

    public Dictionary<string, int> Labels => _lazyLabels.Value;
}