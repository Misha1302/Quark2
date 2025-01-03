using AbstractExecutor;
using CommonDataStructures;

namespace VirtualMachine.Vm.Execution.Executors;

public class Engine(ExecutorConfiguration configuration)
{
    public EngineRuntimeData EngineRuntimeData { get; private set; } = null!;

    public List<AnyOpt> RunFunction(VmModule module, string funcNameToRun, Span<Any> funcArgs)
    {
        var output = new List<AnyOpt>();

        InitRuntimeDataIfNeed(module);
        InitMainInterpreter(module, funcNameToRun, funcArgs);
        ExecuteEveryInterpreter(output);

        return output;
    }

    private void InitMainInterpreter(VmModule module, string funcNameToRun, Span<Any> funcArgs)
    {
        var item = new Interpreter();
        item.Frames.Push(new VmFuncFrame(module[funcNameToRun]));

        var args = funcArgs.ToArray().Select(x => x.MakeAnyOptList()[0]).ToArray();
        item.Stack.PushMany(args);

        EngineRuntimeData.Interpreters.Add(item);
    }

    private void InitRuntimeDataIfNeed(VmModule module)
    {
        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
        EngineRuntimeData ??= new EngineRuntimeData(module, null, [], configuration);
    }

    private void ExecuteEveryInterpreter(List<AnyOpt> output)
    {
        while (EngineRuntimeData.Interpreters.Count > 0)
        {
            foreach (var interpreter in EngineRuntimeData.Interpreters)
            {
                EngineRuntimeData.CurInterpreter = interpreter;
                interpreter.Step(1000, EngineRuntimeData);
            }

            RemoveHaltedInterpreters(output);
        }
    }

    private void RemoveHaltedInterpreters(List<AnyOpt> output)
    {
        for (var i = EngineRuntimeData.Interpreters.Count - 1; i >= 0; i--)
            if (EngineRuntimeData.Interpreters[i].Halted)
            {
                output.Add(EngineRuntimeData.Interpreters[i].Stack.Pop());
                EngineRuntimeData.Interpreters.RemoveAt(i);
            }
    }
}