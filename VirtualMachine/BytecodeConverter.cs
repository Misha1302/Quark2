using CommonBytecode;
using VirtualMachine.Vm.DataStructures;
using VirtualMachine.Vm.DataStructures.VmValues;
using VirtualMachine.Vm.Operations;

namespace VirtualMachine;

public class BytecodeConverter
{
    public VmModule MakeVmModule(BytecodeModule bytecodeModule)
    {
        var functions = bytecodeModule.Functions.Select(ConvertFunction).ToList();
        var vmModule = new VmModule(functions);
        return vmModule;
    }

    private VmFunction ConvertFunction(BytecodeFunction function)
    {
        var ops = ConvertBytecodeToOperations(function.Code);
        var name = function.Name;
        var locals = ExtractLocals(function.Code);
        return new VmFunction(ops, name, locals);
    }

    private List<Operation> ConvertBytecodeToOperations(Bytecode functionCode)
    {
        var ops = new List<Operation>();
        foreach (var instruction in functionCode.Instructions)
            ops.Add(new Operation(instruction.Type, ConvertToVmValues(instruction.Arguments)));
        return ops;
    }

    private List<VmValue> ConvertToVmValues(List<Any> anies)
    {
        var vmValues = new List<VmValue>();
        foreach (var any in anies)
            if (any.Value is BytecodeVariable) DoNothing();
            else vmValues.AddRange(any.MakeVmValue());

        return vmValues;
    }

    private void DoNothing()
    {
    }

    private List<VmVariable> ExtractLocals(Bytecode functionCode) =>
        GetMakingVariablesInstructions(functionCode).SelectMany(ToVmVariables).ToList();

    private static IEnumerable<Instruction> GetMakingVariablesInstructions(Bytecode functionCode)
    {
        return functionCode.Instructions.Where(x => x.Type == InstructionType.MakeVariables);
    }

    private static IEnumerable<VmVariable> ToVmVariables(Instruction instruction)
    {
        return ToBytecodeVariables(instruction).Select(x => new VmVariable(x.Name, x.Type));
    }

    private static List<BytecodeVariable> ToBytecodeVariables(Instruction instruction)
    {
        return instruction.Arguments.Select(x => x.Get<BytecodeVariable>()).ToList();
    }
}