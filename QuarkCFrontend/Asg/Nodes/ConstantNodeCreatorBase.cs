using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public abstract class ConstantNodeCreatorBase(LexemeType lexemeType, AsgNodeType nodeType) : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Number;

    public void TryBuild(List<AsgNode> nodes, int i)
    {
        if (nodes[i].LexemeType == lexemeType)
            nodes[i].NodeType = nodeType;
    }
}