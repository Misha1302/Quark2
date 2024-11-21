using CommonBytecode;
using VirtualMachine.Vm.Operations;

namespace VirtualMachine.Vm.DataStructures;

public record VmFunction(List<Operation> Ops, string Name, List<VmVariable> Variables, List<Label> Labels)
{
    public VmFunction(List<Operation> Ops, string Name, List<VmVariable> Variables)
        : this(Ops, Name, Variables, CalculateLabels(Ops))
    {
    }

    private static List<Label> CalculateLabels(List<Operation> ops)
    {
        var labels = new List<Label>();
        for (var i = 0; i < ops.Count; i++)
            if (ops[i].Type == InstructionType.Label)
                labels.Add(new Label(ops[i].Args[0].GetRef<string>(), i, ops[i].Args[1].Get<long>()));

        return [..labels.OrderBy(x => x.Index)];
    }
}