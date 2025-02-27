namespace RuntimeHackes;

public static class GcExtensions
{
    private static readonly HashSet<object> _objects = [];

    public static void ForbidToCollectIt(this object obj)
    {
        _objects.Add(obj);
    }
}