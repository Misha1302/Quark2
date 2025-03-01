namespace TranslatorToMsil;

public class TranslatorToMsil : IExecutor
{
    public IEnumerable<Any> RunModule() =>
        RunFunction("Main", []);

    public IEnumerable<Any> RunFunction(string name, Span<Any> functionArguments)
    {
        Throw.AssertAlways(RuntimeLibrary.RuntimeData != null, "Module was not initialized");
        foreach (var argument in functionArguments)
            RuntimeLibrary.RuntimeData.IntermediateData.Push(argument.MakeAnyOpt());
        var result = RuntimeLibrary.CallFunc(name);
        return [result.ToAny()];
    }

    public void Init(ExecutorConfiguration configuration)
    {
        var (dynamicMethods, constants) = new Translator().CompileModule(configuration.Module);
        var compiledMethods = new Compiler().Compile(dynamicMethods);
        var methodsDict = compiledMethods.ToDictionary(x => x.Name, x => x.Pointer).ToFrozenDictionary();
        var intermediateData = new OptimizedStack<AnyOpt>();

        RuntimeLibrary.RuntimeData = new TranslatorRuntimeData(constants.ToArray(), methodsDict, intermediateData);
    }
}