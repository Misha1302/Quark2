namespace TranslatorToMsil;

public static class MsilSharpInteractioner
{
    public static unsafe Any CallStaticSharpFunction(
        Any[] args, nint ptr, bool returnsValue
    )
    {
        var stack = new ReversedStackOfAnies(args.ToArray());

        if (returnsValue) return ((delegate*<IReadOnlyStack<Any>, Any>)ptr)(stack);

        ((delegate*<IReadOnlyStack<Any>, void>)ptr)(stack);
        return Any.Nil;
    }

    private class ReversedStackOfAnies(Any[] stack) : IReadOnlyStack<Any>
    {
        public Any Get(int ind)
        {
            var ind2 = stack.Length - (ind > 0 ? ind : stack.Length + ind) - 1;
            Throw.AssertDebug(ind2 >= 0 && ind2 < stack.Length);
            return stack[ind2];
        }
    }
}