﻿namespace VirtualMachine.Vm.Preparing;

public class BytecodeConverter
{
    public VmModule MakeVmModule(BytecodeModule bytecodeModule)
    {
        var functions = bytecodeModule.Functions.Select(function => ConvertFunction(function, bytecodeModule.Functions))
            .ToList();
        var vmModule = new VmModule(functions);
        return vmModule;
    }

    private VmFunction ConvertFunction(BytecodeFunction function, List<BytecodeFunction> functions)
    {
        var ops = function.Code.Instructions.Select(instruction =>
            new VmOperation(instruction.Type, ConvertToVmValues(instruction.Arguments))
        ).ToList();

        var labels = LabelsCalculator.CalculateLabels(function.Code.Instructions);
        PreprocessBranches(ops, labels, functions);

        var name = function.Name;
        var locals = ExtractLocals(function.Code);

        ConvertLocalsOperations(ops, locals);

        return new VmFunction(ops, name, locals, labels);
    }


    private void ConvertLocalsOperations(List<VmOperation> ops, List<VmVariable> locals)
    {
        foreach (var op in ops)
            if (op.Type is LoadLocal or SetLocal)
            {
                var localName = op.Args[0].GetRef<string>();
                var index = (long)locals.FindIndex(x => x.Name == localName);
                Throw.AssertAlways(index >= 0, $"{localName} was not found");
                op.Args[0] = AnyOpt.Create(index, NativeI64);
            }
    }

    private void PreprocessBranches(List<VmOperation> ops, List<VmLabel> labels, List<BytecodeFunction> functions)
    {
        foreach (var op in ops)
            if (op.Type is Br)
            {
                // int - jump ip = [string - jump label name]
                var labelName = op.Args[1].GetRef<string>();
                var index = (long)labels.FindIndex(x => x.Name == labelName);
                Throw.AssertAlways(index >= 0, $"label {labelName} was not found");
                op.Args[1] = AnyOpt.Create(index, NativeI64);
            }
            else if (op.Type is CallFunc)
            {
                var funcName = op.Args[0].GetRef<string>();
                var index = (long)functions.FindIndex(x => x.Name == funcName);
                Throw.AssertAlways(index >= 0, "function {funcName} was not found");
                op.Args[0] = AnyOpt.Create(index, NativeI64);
            }
    }

    private List<AnyOpt> ConvertToVmValues(List<Any> anies)
    {
        var vmValues = new List<AnyOpt>();
        foreach (var any in anies)
            if (any.Value is BytecodeVariable) DoNothing();
            else vmValues.AddRange(any.MakeAnyOptList());

        return vmValues;
    }

    private void DoNothing()
    {
    }

    private List<VmVariable> ExtractLocals(Bytecode functionCode) =>
        GetMakingVariablesInstructions(functionCode).SelectMany(ToVmVariables).ToList();

    private static IEnumerable<BytecodeInstruction> GetMakingVariablesInstructions(Bytecode functionCode)
    {
        return functionCode.Instructions.Where(x => x.Type == MakeVariables);
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