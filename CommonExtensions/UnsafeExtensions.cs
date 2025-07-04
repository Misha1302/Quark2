namespace CommonExtensions;

public static unsafe class UnsafeExtensions
{
    public static void UnsafeCopyTo<T>(this Span<T> a, Span<T> b)
    {
        if (a.Length == 0) return;

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
        fixed (T* p1 = &a[0])
        fixed (T* p2 = &b[0])
            Buffer.MemoryCopy(p1, p2, sizeof(T) * b.Length, sizeof(T) * a.Length);
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
    }
}