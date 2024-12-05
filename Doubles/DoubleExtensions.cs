namespace Doubles;

public static class DoubleExtensions
{
    public static long ToLong(this double value) => (long)(value + 0.5);

    public static bool EqualWithAccuracy(this double a, double b, double accuracy) =>
        Math.Abs(a - b) <= Math.Max(Math.Abs(a), Math.Abs(b)) * accuracy;
}