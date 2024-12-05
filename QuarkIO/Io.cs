using CommonBytecode.Data.AnyValue;
using ExceptionsManager;

namespace QuarkIO;

public static class Io
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

    public static Any InputStr() => Console.ReadLine() ?? Throw.InvalidOpEx<string>("Input was null.");
    public static Any InputNumber() => double.Parse(InputStr().Get<string>());
}