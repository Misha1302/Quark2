using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public class ReturnNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Return;

    public int TryBuild(List<AsgNode> nodes, int i, AsgBuilder asgBuilder)
    {
        if (nodes[i].LexemeType != LexemeType.Return) return 0;

        nodes[i].Children.Add(nodes[i + 1]);
        nodes[i].NodeType = AsgNodeType.Return;
        nodes.RemoveAt(i + 1);
        return 0;
    }
}