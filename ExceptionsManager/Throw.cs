using System.Diagnostics.CodeAnalysis;

namespace ExceptionsManager;

public static class Throw
{
    public static void InvalidOpEx() => throw new InvalidOperationException();

    public static void AssertationFail() => throw new InvalidCastException("Assertion failed");

    public static T InvalidOpEx<T>() => throw new InvalidOperationException();

    public static void Assert([DoesNotReturnIf(false)] bool cond)
    {
#if DEBUG
        if (!cond) AssertationFail();
#endif
    }
}