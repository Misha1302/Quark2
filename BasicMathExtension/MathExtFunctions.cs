namespace BasicMathExtension;

public class MathExtFunctions
{
    public static void AddFunc<T>(T a, T b, out T result) where T : IAddable<T>
    {
        result = T.Add(a, b);
    }
}