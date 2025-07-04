using GenericBytecode.Interfaces;

namespace BasicMathExtension.DefaultImplementations;

public readonly record struct Number(double Value)
    : IBasicValue, IArithmeticOperable<Number>, IModable<Number>
{
    public static Number Add(Number a, Number b) => new(a.Value + b.Value);
    public static Number Sub(Number a, Number b) => new(a.Value - b.Value);
    public static Number Mul(Number a, Number b) => new(a.Value * b.Value);
    public static Number Div(Number a, Number b) => new(a.Value / b.Value);

    public bool Equals(Number other) => Math.Abs(Value - other.Value) < 1e-10;
    public static Number Mod(Number a, Number b) => new(a.Value % b.Value);

    public override int GetHashCode() => HashCode.Combine(Value);
}