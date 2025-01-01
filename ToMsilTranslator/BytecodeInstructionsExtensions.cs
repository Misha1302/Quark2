using CommonDataStructures;

namespace ToMsilTranslator;

public static class BytecodeInstructionsExtensions
{
    public static Dictionary<string, GroboIL.Local> GetLocals(this BytecodeFunction function, GroboIL il)
    {
        var locals = function.Code.Instructions
            .Where(x => x.Type == InstructionType.SetLocal)
            .Select(x => x.Arguments[0].Get<string>())
            .Distinct();

        return locals.ToDictionary(local => local, _ => il.DeclareLocal(typeof(AnyOpt)));
    }

    public static Dictionary<string, GroboIL.Label> GetLabels(this BytecodeFunction function, GroboIL il)
    {
        var labels = function.Code.Instructions
            .Where(x => x.Type == InstructionType.Label)
            .Select(x => x.Arguments[0].Get<string>());

        return labels.ToDictionary(label => label, label => il.DefineLabel(label));
    }
}