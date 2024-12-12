using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public abstract class ConstantNodeCreatorBase(LexemeType lexemeType, AsgNodeType nodeType) : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Number;

    public int TryBuildImpl(List<AsgNode> nodes, int i, AsgBuilder asgBuilder)
    {
        if (nodes[i].LexemeType == lexemeType)
            nodes[i].NodeType = nodeType;
        return 0;
    }
}