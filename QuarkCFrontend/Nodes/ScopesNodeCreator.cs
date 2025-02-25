using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;

namespace QuarkCFrontend.Nodes;

public class ScopesNodeCreator : INodeCreator<QuarkLexemeType>
{
    private readonly List<(QuarkLexemeType left, QuarkLexemeType right)> _scopes =
    [
        (QuarkLexemeType.LeftPar, QuarkLexemeType.RightPar),
        (QuarkLexemeType.LeftBrace, QuarkLexemeType.RightBrace),
        (QuarkLexemeType.LeftBracket, QuarkLexemeType.RightBracket),
    ];

    public AsgNodeType NodeType => AsgNodeType.Scope;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
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
                    TryBuildImpl(nodes, i);

                if (nodes[i].LexemeType == pair.right)
                    break;

                i++;
            }

            var to = i - 1;

            var node = new AsgNode<QuarkLexemeType>(AsgNodeType.Scope, null!, [], nodes[i].LineNumber);
            node.Children.AddRange(nodes[from..(to + 1)]);

            nodes.RemoveRange(from - 1, to - from + 1 + 2);
            nodes.Insert(from - 1, node);
        }

        return 0;
    }
}