namespace CommonBytecode;

public static class Throw
{
    public static T InvalidOpEx<T>() => throw new InvalidOperationException();
}