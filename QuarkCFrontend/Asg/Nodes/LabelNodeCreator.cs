using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public class LabelNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Label;

    public int TryBuildImpl(List<AsgNode> nodes, int i, AsgBuilder asgBuilder)
    {
        if (nodes[i].LexemeType == LexemeType.Label)
            nodes[i].NodeType = AsgNodeType.Label;
        return 0;
    }
}