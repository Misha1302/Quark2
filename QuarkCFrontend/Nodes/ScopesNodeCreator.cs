namespace QuarkCFrontend.Nodes;

public class ScopesNodeCreator : INodeCreator<QuarkLexemeType>
{
    private readonly List<(QuarkLexemeType left, QuarkLexemeType right)> _scopes =
    [
        (LeftPar, RightPar),
        (LeftBrace, RightBrace),
        (LeftBracket, RightBracket),
    ];

    public AsgNodeType NodeType => AsgNodeType.Scope;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var pair in _scopes)
        {
            if (i >= nodes.Count || nodes[i].NodeType == AsgNodeType.Scope || nodes[i].LexemeType != pair.left)
                continue;

            i++;
            var from = i;
            while (true)
            {
                if (nodes[i].LexemeType == pair.left)
                    TryBuildImpl(nodes, i);

                if (nodes[i].LexemeType == pair.right)
                    break;

                i++;
            }

            var to = i - 1;

            var node = new AsgNode<QuarkLexemeType>(
                AsgNodeType.Scope,
                nodes[from - 1].LexemeValue,
                [],
                nodes[i].LineNumber
            );
            node.Children.AddRange(nodes[from..(to + 1)]);

            nodes.RemoveRange(from - 1, to - from + 1 + 2);
            nodes.Insert(from - 1, node);
        }

        return 0;
    }
}