using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public class LoadIdentifierNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Identifier;

    public int TryBuildImpl(List<AsgNode> nodes, int i)
    {
        if (nodes[i].NodeType == AsgNodeType.Unknown && nodes[i].LexemeType == LexemeType.Identifier)
            nodes[i].NodeType = AsgNodeType.Identifier;

        return 0;
    }
}