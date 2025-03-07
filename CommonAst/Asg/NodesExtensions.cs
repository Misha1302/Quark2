namespace DefaultAstImpl.Asg;

public static class NodesExtensions
{
    private static readonly int[] _primes6Digits = [866857, 319763, 839269, 110311, 845003, 529003];

    private static readonly Int128[] _primes25Digits =
    [
        Int128.Parse("4303962481239994761716573"),
        Int128.Parse("9504930567202207527583177"),
        Int128.Parse("9983880348697094897557063"),
        Int128.Parse("1197288366761786054652553"),
        Int128.Parse("4617827220735272856201497"),
    ];

    private static readonly int _k = _primes6Digits[Random.Shared.Next(_primes6Digits.Length)];
    private static readonly Int128 _mod = _primes25Digits[Random.Shared.Next(_primes25Digits.Length)];

    public static Int128 CalcHashCodeForNodes<T>(this IReadOnlyList<AsgNode<T>> nodes)
    {
        var hashCode = (Int128)1;
        var pow = (Int128)1;
        CalcHashCodeForNodes(nodes, ref hashCode, ref pow);
        return hashCode;
    }

    public static void CalcHashCodeForNodes<T>(
        this IReadOnlyList<AsgNode<T>> nodes,
        ref Int128 hashCode,
        ref Int128 pow
    )
    {
        pow = Int128.Max(1, pow);
        foreach (var node in nodes)
        {
            CalcHashCodeForNodes(node.Children, ref hashCode, ref pow);
            var x = node.CalcHashCodeForNode();
            hashCode = (hashCode + x * pow) % _mod;
            pow = pow * _k % _mod;
        }
    }

    public static Int128 CalcHashCodeForNode<T>(this AsgNode<T> node) =>
        ((long)node.NodeType + 1) * (node.Children.Count + (long)AsgNodeType.MaxEnumValue + 1);
}