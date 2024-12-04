namespace ExceptionsManager;

public static class Throw
{
    public static void InvalidOpEx()
    {
        throw new InvalidOperationException();
    }

    public static T InvalidOpEx<T>() => throw new InvalidOperationException();

    public static void Assert(bool cond)
    {
#if DEBUG
        if (!cond) InvalidOpEx();
#endif
    }
}