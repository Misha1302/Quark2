using CommonLoggers;
using ExceptionsManager;
using GenericBytecode;
using GenericBytecode.Structures;

namespace GenericBytecodeVirtualMachine;

public class Interpreter(ILogger logger)
{
    private readonly Stack<FunctionFrame> _functionStack = new();
    private readonly Stack<IBasicValue> _valuesStack = new();

    private FunctionFrame CurrentFrame => _functionStack.Peek();

    public IEnumerable<IBasicValue> Run()
    {
        while (_functionStack.Count > 0)
        {
            while (CurrentFrame.Sp >= 0)
                Step();

            _functionStack.Pop();
        }

        return _valuesStack;
    }

    private void Step()
    {
        var instruction = CurrentFrame.Bytecode.Body.Instructions[CurrentFrame.Sp];
        CurrentFrame.Sp++;

        Throw.AssertAlways(instruction.Value != InstructionValue.Invalid, "Invalid instruction value");
        if (instruction.Value == InstructionValue.Ret)
            CurrentFrame.Sp = -1;
        else if (instruction.Value == InstructionValue.JumpIfTrue)
            JumpIfTrue(instruction);
        else if (instruction.Value == InstructionValue.SetLabel)
            Nop();
        else CallInstruction(instruction);

        if (logger.GetType() != typeof(PlugLogger))
            logger.Log(
                "Step",
                $"Executed: {instruction}".PadRight(115) + "[ " + string.Join(", ", _valuesStack) + " ]"
            );
    }

    private void JumpIfTrue(Instruction instruction)
    {
        if (_valuesStack.Pop().To<IBasicValue, IBoolean>().ToBool())
            CurrentFrame.Sp = CurrentFrame.Labels[instruction.Args[0].Invoke<Str>()];
    }

    private void Nop()
    {
    }

    private void CallInstruction(Instruction instruction)
    {
        foreach (var act in instruction.Args)
        {
            var args = GenericArrayPool<object?>.Shared.Rent(act.Parameters.Length);

            // parameters should be loaded in reverse order
            for (var i = 0; i < act.ParametersWithoutRefs.Length; i++)
                args[act.ParametersWithoutRefs.Length - 1 - i] = _valuesStack.Pop();

            // we need to rewrite garbage
            for (var i = 0; i < act.ParametersRefs.Length; i++)
                args[i + act.ParametersWithoutRefs.Length] = null;

            act.Invoke(args);

            // push received values on stack
            for (var i = 0; i < act.Parameters.Length; i++)
                if (act.Parameters[i].ParameterType.IsByRef)
                    _valuesStack.Push((IBasicValue)args[i]!);

            GenericArrayPool<object?>.Shared.Return(args);
        }
    }

    public void AddFunction(FunctionFrame function)
    {
        _functionStack.Push(function);
    }
}