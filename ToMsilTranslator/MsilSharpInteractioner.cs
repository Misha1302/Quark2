using CommonDataStructures;
using Any = CommonBytecode.Data.AnyValue.Any;

namespace ToMsilTranslator;

public class MsilSharpInteractioner
{
    public static unsafe Any CallStaticSharpFunction(
        Any[] args, nint ptr, bool returnsValue
    )
    {
        var stack = new StackOfAnies(args.Reverse().ToArray());

        if (returnsValue) return ((delegate*<IReadOnlyStack<Any>, Any>)ptr)(stack);

        ((delegate*<IReadOnlyStack<Any>, void>)ptr)(stack);
        return Any.Nil;
    }

    public class StackOfAnies(Any[] stack) : IReadOnlyStack<Any>
    {
        public Any Get(int ind)
        {
            var ind2 = ind > 0 ? ind : stack.Length + ind;
            Throw.AssertDebug(ind2 >= 0 && ind2 < stack.Length);
            return stack[ind2];
        }
    }
}