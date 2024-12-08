namespace QuarkCFrontend;

public class ScopesNodeCreator : INodeCreator
{
    private readonly List<(LexemeType left, LexemeType right)> _scopes =
    [
        (LexemeType.LeftPar, LexemeType.RightPar),
        (LexemeType.LeftBrace, LexemeType.RightBrace),
        (LexemeType.LeftBracket, LexemeType.RightBracket),
    ];

    public AsgNodeType NodeType => AsgNodeType.Scope;

    public void TryBuild(List<AsgNode> nodes, int i)
    {
        foreach (var pair in _scopes)
        {
            if (i >= nodes.Count || nodes[i].LexemeType != pair.left)
                continue;

            i++;
            var from = i;
            while (true)
            {
                if (nodes[i].LexemeType == pair.left)
                    TryBuild(nodes, i);

                if (nodes[i].LexemeType == pair.right)
                    break;

                i++;
            }

            var to = i - 1;

            var node = new AsgNode(AsgNodeType.Scope, null!, []);
            node.Children.AddRange(nodes[from..(to + 1)]);

            nodes.RemoveRange(from - 1, to - from + 1 + 2);
            nodes.Insert(from - 1, node);
        }
    }
}