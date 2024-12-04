namespace VirtualMachine.Vm.DataStructures;

/// <summary>
///     Presents bytecode Label information
/// </summary>
/// <param name="Name">name of label</param>
/// <param name="Ip">instruction pointer of label</param>
public record Label(string Name, int Ip);