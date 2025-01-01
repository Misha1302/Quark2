using AbstractExecutor;
using CommonDataStructures;

namespace VirtualMachine.Vm.Execution.Executors;

public class Engine(ExecutorConfiguration configuration)
{
    public EngineRuntimeData EngineRuntimeData { get; private set; } = null!;

    public List<AnyOpt> Run(VmModule module,
        Action<VmOperation, int, VmFuncFrame, MyStack<AnyOpt>>? logAction = null)
    {
        var output = new List<AnyOpt>();

        InitRuntimeData(module, logAction);
        InitMainInterpreter(module);
        ExecuteEveryInterpreter(output);

        return output;
    }

    private void InitMainInterpreter(VmModule module)
    {
        var item = new Interpreter();
        item.Frames.Push(new VmFuncFrame(module["Main"]));
        EngineRuntimeData.Interpreters.Add(item);
    }

    private void InitRuntimeData(VmModule module, Action<VmOperation, int, VmFuncFrame, MyStack<AnyOpt>>? logAction)
    {
        EngineRuntimeData = new EngineRuntimeData(module, logAction, [], configuration);
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