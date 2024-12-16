using AbstractExecutor;

namespace VirtualMachine.Vm.Execution.Executors;

public class Engine(ExecutorConfiguration configuration)
{
    private EngineRuntimeData _engineRuntimeData = null!;

    public List<VmValue> Run(VmModule module,
        Action<VmOperation, int, VmFuncFrame, MyStack<VmValue>>? logAction = null)
    {
        var output = new List<VmValue>();

        InitRuntimeData(module, logAction);
        InitMainInterpreter(module);
        ExecuteEveryInterpreter(output);

        return output;
    }

    private void InitMainInterpreter(VmModule module)
    {
        var item = new Interpreter();
        item.Frames.Push(new VmFuncFrame(module["Main"]));
        _engineRuntimeData.Interpreters.Add(item);
    }

    private void InitRuntimeData(VmModule module, Action<VmOperation, int, VmFuncFrame, MyStack<VmValue>>? logAction)
    {
        _engineRuntimeData = new EngineRuntimeData(module, logAction, [], configuration);
    }

    private void ExecuteEveryInterpreter(List<VmValue> output)
    {
        while (_engineRuntimeData.Interpreters.Count > 0)
        {
            foreach (var interpreter in _engineRuntimeData.Interpreters)
            {
                _engineRuntimeData.CurInterpreter = interpreter;
                interpreter.Step(1000, _engineRuntimeData);
            }

            RemoveHaltedInterpreters(output);
        }
    }

    private void RemoveHaltedInterpreters(List<VmValue> output)
    {
        for (var i = _engineRuntimeData.Interpreters.Count - 1; i >= 0; i--)
            if (_engineRuntimeData.Interpreters[i].Halted)
            {
                output.Add(_engineRuntimeData.Interpreters[i].Stack.Pop());
                _engineRuntimeData.Interpreters.RemoveAt(i);
            }
    }
}