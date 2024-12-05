namespace VirtualMachine.Vm.Execution;

public static class SharpInteractioner
{
    public static unsafe void CallStaticSharpFunction(
        MyStack<VmValue> stack, nint ptr, long argsCount, bool returnsValue
    )
    {
        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/function-pointers
        // use function-pointers to call it faster than usual csharp delegates
        if (!returnsValue)
        {
            // call with no return value
            if (argsCount == 0)
                ((delegate*<void>)ptr)();
            else if (argsCount == 1)
                ((delegate*<Any, void>)ptr)(stack.Get(-1).ToAny());
            else if (argsCount == 2)
                ((delegate*<Any, Any, void>)ptr)(stack.Get(-2).ToAny(), stack.Get(-1).ToAny());
            else if (argsCount == 3)
                ((delegate*<Any, Any, Any, void>)ptr)(stack.Get(-3).ToAny(), stack.Get(-2).ToAny(),
                    stack.Get(-1).ToAny());
            else Throw.InvalidOpEx();

            stack.DropMany(argsCount);
        }
        else
        {
            // call with Any return type
            var result = argsCount switch
            {
                0 => ((delegate*<Any>)ptr)(),
                1 => ((delegate*<Any, Any>)ptr)(stack.Get(-1).ToAny()),
                2 => ((delegate*<Any, Any, Any>)ptr)(stack.Get(-2).ToAny(), stack.Get(-1).ToAny()),
                3 => ((delegate*<Any, Any, Any, Any>)ptr)(stack.Get(-3).ToAny(), stack.Get(-2).ToAny(),
                    stack.Get(-1).ToAny()),
                _ => Throw.InvalidOpEx<Any>(),
            };

            stack.DropMany(argsCount);

            stack.PushMany(CollectionsMarshal.AsSpan(result.MakeVmValue()));
        }
    }
}