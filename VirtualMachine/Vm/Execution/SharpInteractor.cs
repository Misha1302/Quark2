namespace VirtualMachine.Vm.Execution;

public static class SharpInteractor
{
    public static unsafe void CallStaticSharpFunction(
        ExtendedStack<AnyOpt> stack, nint ptr, long argsCount, bool returnsValue, bool isVarArgs
    )
    {
        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/function-pointers
        // use function-pointers to call it faster than usual csharp delegates
        if (!returnsValue)
        {
            // call with no return value
            if (isVarArgs)
            {
                var argsCountStack = stack.Get(-1).Get<double>().ToLong();
                ((delegate*<IReadOnlyStack<Any>, void>)ptr)(new StackOfAnies(stack));
                stack.DropMany(argsCountStack + 1);
            }
            else
            {
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

            stack.Push(AnyOpt.NilValue);
        }
        else
        {
            // call with Any return type
            Any result;
            if (!isVarArgs)
            {
                result = argsCount switch
                {
                    0 => ((delegate*<Any>)ptr)(),
                    1 => ((delegate*<Any, Any>)ptr)(stack.Get(-1).ToAny()),
                    2 => ((delegate*<Any, Any, Any>)ptr)(stack.Get(-2).ToAny(), stack.Get(-1).ToAny()),
                    3 => ((delegate*<Any, Any, Any, Any>)ptr)(stack.Get(-3).ToAny(), stack.Get(-2).ToAny(),
                        stack.Get(-1).ToAny()),
                    _ => Throw.InvalidOpEx<Any>(),
                };

                stack.DropMany(argsCount);
            }
            else
            {
                var argsCountStack = stack.Get(-1).Get<double>().ToLong();
                result = ((delegate*<IReadOnlyStack<Any>, Any>)ptr)(new StackOfAnies(stack));
                stack.DropMany(argsCountStack + 1);
            }

            stack.PushMany(CollectionsMarshal.AsSpan(result.MakeAnyOptList()));
        }
    }
}