namespace VirtualMachine.Vm.Preparing;

public static class LabelsCalculator
{
    public static List<Label> CalculateLabels(List<BytecodeInstruction> ops)
    {
        var labels = new List<Label>();
        for (var i = 0; i < ops.Count; i++)
            if (ops[i].Type == InstructionType.Label)
                labels.Add(new Label(ops[i].Arguments[0].Get<string>(), i));

        return labels;
    }
}