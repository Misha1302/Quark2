using CommonBytecode;

namespace Quark2;

public static class BuiltInFunctions
{
    public static void PrintLn(Any value)
    {
        Print(value);
        Console.WriteLine();
    }

    public static void Print(Any value)
    {
        Console.Write(value.ToString());
    }
}