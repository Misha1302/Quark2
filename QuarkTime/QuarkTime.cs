using SharpAnyType;

namespace QuarkTime;

public static class QuarkTime
{
    public static Any GetTimeInMilliseconds() => DateTimeOffset.Now.ToUnixTimeMilliseconds();
}