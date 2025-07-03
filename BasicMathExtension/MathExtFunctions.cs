namespace BasicMathExtension;

public class MathExtFunctions
{
    public static void AddFunc<T>(T a, T b, out T result) where T : IAddable<T> => result = T.Add(a, b);
    public static void SubFunc<T>(T a, T b, out T result) where T : ISubable<T> => result = T.Sub(a, b);
    public static void MulFunc<T>(T a, T b, out T result) where T : IMulable<T> => result = T.Mul(a, b);
    public static void DivFunc<T>(T a, T b, out T result) where T : IDivable<T> => result = T.Div(a, b);
    public static void ModFunc<T>(T a, T b, out T result) where T : IModable<T> => result = T.Mod(a, b);
}