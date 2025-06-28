﻿namespace ExceptionsManager;

public static class Throw
{
    [DoesNotReturn]
    public static void InvalidOpEx(string message = "") => throw new InvalidOperationException(message);

    [DoesNotReturn]
    public static void AssertationFail(string errorMessage = "") => InvalidOpEx($"Assertion failed: {errorMessage}");

    [DoesNotReturn]
    public static T InvalidOpEx<T>(string message = "") => throw new InvalidOperationException(message);

    // [Conditional("DEBUG")]
    public static void AssertAlways(
        [DoesNotReturnIf(false)] bool cond,
        string errorMessage = "",
        [CallerArgumentExpression(nameof(cond))]
        string expression = ""
    )
    {
        if (!cond) AssertationFail(errorMessage == "" ? expression : errorMessage);
    }

    public static void NotImplementedException(string msg = "") => throw new NotImplementedException(msg);

    [DoesNotReturn]
    public static T NullException<T>() => throw new NullReferenceException();
}