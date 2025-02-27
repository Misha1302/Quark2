namespace VirtualMachine.Vm.Preparing;

public static class LabelsCalculator
{
    public static List<VmLabel> CalculateLabels(List<BytecodeInstruction> ops)
    {
        var labels = new List<VmLabel>();
        for (var i = 0; i < ops.Count; i++)
            if (ops[i].Type == InstructionType.Label)
                labels.Add(new VmLabel(ops[i].Arguments[0].Get<string>(), i));

        return labels;
    }
}