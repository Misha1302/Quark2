using CommonBytecode.Data.AnyValue;
using Doubles;

namespace QuarkVector;

public static class VectorOperations
{
    public static Any CreateVector() =>
        new(new VectorCollection<Any>());

    public static void SetSize(Any vector, Any length) =>
        vector.Get<VectorCollection<Any>>().SetSize((int)length.Get<double>().ToLong());

    public static void SetValue(Any vector, Any index, Any value) =>
        vector.Get<VectorCollection<Any>>()[(int)index.Get<double>().ToLong()] = value;

    public static Any GetValue(Any vector, Any index) =>
        vector.Get<VectorCollection<Any>>()[(int)index.Get<double>().ToLong()];

    public static Any GetSize(Any vector) =>
        vector.Get<VectorCollection<Any>>().Count;

    public static void SwapValues(Any vectorAny, Any index1Any, Any index2Any)
    {
        var index1 = (int)index1Any.Get<double>().ToLong();
        var index2 = (int)index2Any.Get<double>().ToLong();
        var vector = vectorAny.Get<VectorCollection<Any>>();

        (vector[index1], vector[index2]) = (vector[index2], vector[index1]);
    }
}