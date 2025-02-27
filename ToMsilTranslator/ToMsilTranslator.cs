namespace ToMsilTranslator;

public class ToMsilTranslator : IExecutor
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
        var (methods, constants) = new Compiler().CompileModule(configuration.Module);
        RuntimeLibrary.RuntimeData =
            new ToMsilTranslatorRuntimeData(
                constants,
                methods.ToDictionary(x => x.Name, x => x),
                new Stack<AnyOpt>()
            );
    }
}