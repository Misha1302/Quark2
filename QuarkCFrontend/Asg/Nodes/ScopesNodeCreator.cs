using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public class ScopesNodeCreator : INodeCreator
{
    private readonly List<(LexemeType left, LexemeType right)> _scopes =
    [
        (LexemeType.LeftPar, LexemeType.RightPar),
        (LexemeType.LeftBrace, LexemeType.RightBrace),
        (LexemeType.LeftBracket, LexemeType.RightBracket),
    ];

    public AsgNodeType NodeType => AsgNodeType.Scope;

    public int TryBuild(List<AsgNode> nodes, int i, AsgBuilder asgBuilder)
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
                    TryBuild(nodes, i, asgBuilder);

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

        return 0;
    }
}