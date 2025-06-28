using ExceptionsManager;

namespace GenericBytecode2.Structures;

public static class ConvertorExtensions
{
    public static TTo To<TFrom, TTo>(this TFrom v) =>
        (TTo)((object?)v ?? Throw.NullException<TTo>());
}