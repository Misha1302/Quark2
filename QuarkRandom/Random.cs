using CommonBytecode.Data.AnyValue;
using Doubles;

namespace QuarkRandom;

public static class Random
{
    public static Any GetRandomInteger(Any min, Any max) =>
        System.Random.Shared.NextInt64(min.Get<double>().ToLong(), max.Get<double>().ToLong());
}