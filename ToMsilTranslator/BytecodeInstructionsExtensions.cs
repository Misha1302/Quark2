using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
using GrEmit;

namespace ToMsilTranslator;

public static class BytecodeInstructionsExtensions
{
    public static Dictionary<string, GroboIL.Local> GetLocals(this BytecodeFunction function, GroboIL il)
    {
        var locals = function.Code.Instructions
            .Where(x => x.Type == InstructionType.SetLocal)
            .Select(x => x.Arguments[0].Get<string>())
            .Distinct();

        return locals.ToDictionary(local => local, _ => il.DeclareLocal(typeof(Any)));
    }

    public static Dictionary<string, GroboIL.Label> GetLabels(this BytecodeFunction function, GroboIL il)
    {
        var labels = function.Code.Instructions
            .Where(x => x.Type == InstructionType.Label)
            .Select(x => x.Arguments[0].Get<string>());

        return labels.ToDictionary(label => label, label => il.DefineLabel(label));
    }
}