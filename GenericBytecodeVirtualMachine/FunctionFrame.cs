using GenericBytecode2;

namespace GenericBytecodeVirtualMachine;

public class FunctionFrame(GenericBytecodeFunction bytecode)
{
    public readonly GenericBytecodeFunction Bytecode = bytecode;
    public int Sp;
}