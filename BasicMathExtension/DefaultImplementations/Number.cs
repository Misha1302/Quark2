using GenericBytecode.Interfaces;

namespace BasicMathExtension.DefaultImplementations;

public record struct Number(double Value) : IBasicValue, IArithmeticOperable<Number>, IModable<Number>
{
    public static Number Add(Number a, Number b) => new(a.Value + b.Value);
    public static Number Sub(Number a, Number b) => new(a.Value - b.Value);
    public static Number Mul(Number a, Number b) => new(a.Value * b.Value);
    public static Number Div(Number a, Number b) => new(a.Value / b.Value);
    public static Number Mod(Number a, Number b) => new(a.Value % b.Value);
}