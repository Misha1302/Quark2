using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes;

public class ReturnNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Return;

    public int TryBuildImpl(List<AsgNode> nodes, int i)
    {
        if (nodes[i].LexemeType != LexemeType.Return) return 0;

        nodes[i].Children.Add(nodes[i + 1]);
        nodes[i].NodeType = AsgNodeType.Return;
        nodes.RemoveAt(i + 1);
        return 0;
    }
}