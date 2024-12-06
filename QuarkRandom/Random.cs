using CommonBytecode.Data.AnyValue;
using Doubles;

namespace QuarkRandom;

public static class Random
{
    public static Any GetRandomInteger(Any min, Any max)
    {
        var minValue = min.Get<double>().ToLong();
        var maxValue = max.Get<double>().ToLong() + 1;
        // Console.WriteLine($"Min: {minValue}, Max: {maxValue}");
        return System.Random.Shared.NextInt64(minValue, maxValue);
    }
}