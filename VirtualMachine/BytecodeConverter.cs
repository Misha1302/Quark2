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
        var ops = function.Code.Instructions.Select(
            instruction => new Operation(instruction.Type, ConvertToVmValues(instruction.Arguments))
        ).ToList();

        var labels = LabelsCalculator.CalculateLabels(function.Code.Instructions);
        PreprocessBranches(ops, labels);

        var name = function.Name;
        var locals = ExtractLocals(function.Code);

        ConvertLocalsOperations(ops, locals);

        return new VmFunction(ops, name, locals, labels);
    }


    private void ConvertLocalsOperations(List<Operation> ops, List<VmVariable> locals)
    {
        foreach (var op in ops)
            if (op.Type is InstructionType.LoadLocal or InstructionType.SetLocal)
                op.Args[0] = VmValue.Create((long)locals.FindIndex(x => x.Name == op.Args[0].GetRef<string>()),
                    NativeI64);
    }

    private void PreprocessBranches(List<Operation> ops, List<Label> labels)
    {
        foreach (var op in ops)
            if (op.Type is InstructionType.BrOp)
                // int - jump ip = [string - jump label name]
            {
                var findIndex = (long)labels.FindIndex(x => x.Name == op.Args[1].GetRef<string>());
                op.Args[1] = VmValue.Create(findIndex, NativeI64);
            }
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

    private static IEnumerable<BytecodeInstruction> GetMakingVariablesInstructions(Bytecode functionCode)
    {
        return functionCode.Instructions.Where(x => x.Type == InstructionType.MakeVariables);
    }

    private static IEnumerable<VmVariable> ToVmVariables(BytecodeInstruction bytecodeInstruction)
    {
        return ToBytecodeVariables(bytecodeInstruction).Select(x => new VmVariable(x.Name, x.Type));
    }

    private static List<BytecodeVariable> ToBytecodeVariables(BytecodeInstruction bytecodeInstruction)
    {
        return bytecodeInstruction.Arguments.Select(x => x.Get<BytecodeVariable>()).ToList();
    }
}