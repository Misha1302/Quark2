namespace QuarkCFrontend.Asg;

public static class NodesExtensions
{
    private static readonly int[] _primes6Digits = [102059, 866857, 319763, 839269, 110311, 845003, 529003];
    private static readonly int[] _primes7Digits = [4171781, 8957243, 7489127, 5764589, 4346929, 5935649, 6753589];

    private static readonly int _k = _primes6Digits[Random.Shared.Next(_primes6Digits.Length)];
    private static readonly int _mod = _primes7Digits[Random.Shared.Next(_primes7Digits.Length)];

    public static long CalcHashCodeForNodes(this IReadOnlyList<AsgNode> nodes, long prev = 0)
    {
        var hashCode = prev;

        foreach (var node in nodes)
        {
            hashCode = CalcHashCodeForNodes(node.Children, hashCode);
            hashCode = hashCode * _k % _mod + node.CalcHashCodeForNode() % _mod;
        }

        return hashCode;
    }

    public static long CalcHashCodeForNode(this AsgNode node) =>
        (long)node.NodeType * (node.Children.Count + (long)AsgNodeType.MaxEnumValue);
}