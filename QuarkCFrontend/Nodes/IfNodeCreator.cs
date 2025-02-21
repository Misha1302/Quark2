using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes;

public class IfNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.If;

    public int TryBuildImpl(List<AsgNode> nodes, int i)
    {
        if (i + 2 >= nodes.Count) return 0;
        if (nodes[i].LexemeType != LexemeType.If) return 0;

        nodes[i].NodeType = AsgNodeType.If;
        nodes[i].Children.AddRange([nodes[i + 1], nodes[i + 2]]);
        nodes.RemoveAt(i + 2);
        nodes.RemoveAt(i + 1);

        return 0;
    }
}