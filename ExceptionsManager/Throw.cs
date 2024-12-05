using System.Diagnostics.CodeAnalysis;

namespace ExceptionsManager;

public static class Throw
{
    public static void InvalidOpEx(string message = "") => throw new InvalidOperationException(message);

    public static void AssertationFail() => InvalidOpEx("Assertion failed");

    public static T InvalidOpEx<T>(string message = "") => throw new InvalidOperationException(message);

    public static void Assert([DoesNotReturnIf(false)] bool cond)
    {
#if DEBUG
        if (!cond) AssertationFail();
#endif
    }
}