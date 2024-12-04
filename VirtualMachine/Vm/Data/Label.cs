namespace VirtualMachine.Vm.Data;

/// <summary>
///     Presents bytecode Label information
/// </summary>
/// <param name="Name">name of label</param>
/// <param name="Ip">instruction pointer of label</param>
public record Label(string Name, int Ip);