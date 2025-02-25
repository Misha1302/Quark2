namespace ExceptionsManager;

public static class Throw
{
    public static void InvalidOpEx(string message = "") => throw new InvalidOperationException(message);

    public static void AssertationFail(string errorMessage = "") => InvalidOpEx($"Assertion failed: {errorMessage}");

    public static T InvalidOpEx<T>(string message = "") => throw new InvalidOperationException(message);

    [Conditional("DEBUG")]
    public static void AssertDebug(
        [DoesNotReturnIf(false)] bool cond,
        string errorMessage = "",
        [CallerArgumentExpression(nameof(cond))]
        string expression = ""
    )
    {
        AssertAlways(cond, errorMessage, expression);
    }

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
}