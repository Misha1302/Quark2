using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg;

public class LoadIdentifierNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Identifier;

    public int TryBuild(List<AsgNode> nodes, int i, AsgBuilder asgBuilder)
    {
        if (nodes[i].NodeType == AsgNodeType.Unknown && nodes[i].LexemeType == LexemeType.Identifier)
            nodes[i].NodeType = AsgNodeType.Identifier;

        return 0;
    }
}