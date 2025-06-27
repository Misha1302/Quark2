using ExceptionsManager;
using GenericBytecode2;

namespace GenericBytecodeVirtualMachine;

public class Interpreter
{
    private readonly Stack<FunctionFrame> _functionStack = new();
    private readonly Stack<IBasicValue> _valuesStack = new();

    private FunctionFrame CurrentFrame => _functionStack.Peek();

    public IEnumerable<IBasicValue> Run()
    {
        while (_functionStack.Count > 0)
        {
            while (CurrentFrame.Sp >= 0)
            {
                var instruction = CurrentFrame.Bytecode.Body.Instructions[CurrentFrame.Sp];
                CurrentFrame.Sp++;

                Throw.AssertAlways(instruction.Value != InstructionValue.Invalid, "Invalid instruction value");
                if (instruction.Value == InstructionValue.Ret) CurrentFrame.Sp = -1;
                else if (instruction.Value == InstructionValue.Goto) CurrentFrame.Sp = -1;
                else CallInstruction(instruction);

                Console.WriteLine(
                    $"  Executed: {instruction}".PadRight(75) + "[ " + string.Join(", ", _valuesStack) + " ]"
                );
            }

            _functionStack.Pop();
        }

        return _valuesStack;
    }

    private void CallInstruction(Instruction instruction)
    {
        foreach (var act in instruction.Args)
        {
            var args = Enumerable.Range(0, act.Action.Method.GetParameters().Length).Cast<object?>().ToArray();
            var i = 0;
            foreach (var arg in act.Action.Method.GetParameters())
            {
                args[i] = !arg.ParameterType.IsByRef ? _valuesStack.Pop() : null;
                i++;
            }


            act.Action.Method.Invoke(null, args);


            i = 0;
            foreach (var arg in act.Action.Method.GetParameters())
            {
                if (arg.ParameterType.IsByRef) _valuesStack.Push((IBasicValue)args[i]!);
                i++;
            }
        }
    }

    public void AddFunction(FunctionFrame function)
    {
        _functionStack.Push(function);
    }
}