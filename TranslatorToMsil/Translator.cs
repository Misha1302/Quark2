namespace TranslatorToMsil;

public class Translator
{
    public (List<DynamicMethod>, List<AnyOpt>) CompileModule(BytecodeModule module)
    {
        var constants = new List<AnyOpt>();
        var methods = module.Functions.Select(function => CompileFunction(module, function, constants)).ToList();
        return (methods, constants);
    }

    private DynamicMethod CompileFunction(
        BytecodeModule module, BytecodeFunction function, List<AnyOpt> constants
    )
    {
        var dynamicMethod = new DynamicMethod(
            function.Name,
            typeof(AnyOpt),
            [typeof(TranslatorRuntimeData)],
            true
        );

        using var il = new GroboIL(dynamicMethod);

        var data = new FunctionCompileData(
            function.GetLocals(il),
            function.GetLabels(il)
        );

        var parametersCount = function.Code.GetParametersCount();
        for (var i = 0; i < parametersCount; i++) il.Call(DelegatesHelper.GetInfo(RuntimeLibrary.PopFromStack));

        var functionsCompiler = new FunctionsCompiler();
        for (var index = 0; index < function.Code.Instructions.Count; index++)
        {
            var instructions = function.Code.Instructions;
            var instruction = instructions[index];
            var prevInstruction = index - 1 >= 0 ? instructions[index - 1] : null;
            var nextInstruction = index + 1 < instructions.Count ? instructions[index + 1] : null;
            functionsCompiler.CompileInstruction(
                il,
                prevInstruction,
                instruction,
                nextInstruction,
                data,
                module,
                constants
            );
        }

        return dynamicMethod;
    }
}