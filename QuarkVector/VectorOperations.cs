using CommonBytecode.Data.AnyValue;
using CommonDataStructures;
using Doubles;

namespace QuarkVector;

public static class VectorOperations
{
    public static Any CreateVector(IReadOnlyStack<Any> stack)
    {
        var elementsCount = stack.Get(-1).Get<double>().ToLong();
        var vec = new VectorImpl<Any>();
        vec.SetSize(elementsCount);
        for (var i = 0; i < elementsCount; i++) vec[(int)elementsCount - i - 1] = stack.Get(-(i + 2));

        return new Any(vec, BytecodeValueType.SomeSharpObject);
    }

    public static void SetSize(Any vector, Any length) =>
        vector.Get<VectorImpl<Any>>().SetSize(length.Get<double>().ToLong());

    public static void SetValue(Any vector, Any index, Any value) =>
        vector.Get<VectorImpl<Any>>()[(int)index.Get<double>().ToLong()] = value;

    public static Any GetValue(Any vector, Any index) =>
        vector.Get<VectorImpl<Any>>()[(int)index.Get<double>().ToLong()];

    public static Any GetSize(Any vector) =>
        vector.Get<VectorImpl<Any>>().Count;

    public static void SwapValues(Any vectorAny, Any index1Any, Any index2Any)
    {
        var index1 = (int)index1Any.Get<double>().ToLong();
        var index2 = (int)index2Any.Get<double>().ToLong();
        var vector = vectorAny.Get<VectorImpl<Any>>();

        (vector[index1], vector[index2]) = (vector[index2], vector[index1]);
    }
}