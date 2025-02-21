using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes;

public class LabelNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Label;

    public int TryBuildImpl(List<AsgNode> nodes, int i)
    {
        if (nodes[i].LexemeType == LexemeType.Label)
            nodes[i].NodeType = AsgNodeType.Label;
        return 0;
    }
}