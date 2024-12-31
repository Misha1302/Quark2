using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ExceptionsManager;

public static class Throw
{
    public static void InvalidOpEx(string message = "") => throw new InvalidOperationException(message);

    public static void AssertationFail(string errorMessage = "") => InvalidOpEx($"Assertion failed: {errorMessage}");

    public static T InvalidOpEx<T>(string message = "") => throw new InvalidOperationException(message);

    [Conditional("DEBUG")]
    public static void Assert(
        [DoesNotReturnIf(false)] bool cond,
        string errorMessage = "",
        [CallerArgumentExpression(nameof(cond))]
        string expression = ""
    )
    {
        if (!cond) AssertationFail(errorMessage == "" ? expression : errorMessage);
    }

    public static void NotImplementedException() => throw new NotImplementedException();
}