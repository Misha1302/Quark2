namespace TranslatorToMsil;

public static class MsilSharpInteractioner
{
    public static unsafe Any CallVarArgsStaticSharpFunction(
        Any[] args, nint ptr, bool returnsValue
    )
    {
        var stack = new BasicReadonlyStackOfAnies(args);

        if (returnsValue) return ((delegate*<IReadOnlyStack<Any>, Any>)ptr)(stack);

        ((delegate*<IReadOnlyStack<Any>, void>)ptr)(stack);
        return Any.Nil;
    }

    private class BasicReadonlyStackOfAnies(Any[] stack) : IReadOnlyStack<Any>
    {
        public Any Get(int ind)
        {
            var ind2 = ind > 0 ? ind : stack.Length + ind;
            Throw.AssertDebug(ind2 >= 0 && ind2 < stack.Length);
            return stack[ind2];
        }
    }
}