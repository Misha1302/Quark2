using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes;

public abstract class ConstantNodeCreatorBase(LexemeType lexemeType, AsgNodeType nodeType) : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Number;

    public int TryBuildImpl(List<AsgNode> nodes, int i)
    {
        if (nodes[i].LexemeType == lexemeType)
            nodes[i].NodeType = nodeType;
        return 0;
    }
}