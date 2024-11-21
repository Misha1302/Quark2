namespace VirtualMachine.Vm.DataStructures;

/// <summary>
///     Presents bytecode Label information
/// </summary>
/// <param name="Name">name of label</param>
/// <param name="Ip">instruction pointer of label</param>
/// <param name="Index">it's index to use in bytecode. Must be unique</param>
public record Label(string Name, int Ip, long Index);