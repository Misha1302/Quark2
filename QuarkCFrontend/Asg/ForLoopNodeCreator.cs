using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg;

public class ForLoopNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.ForLoop;

    public int TryBuild(List<AsgNode> nodes, int i, AsgBuilder asgBuilder)
    {
        if (i + 4 >= nodes.Count) return 0;
        if (nodes[i].LexemeType != LexemeType.For) return 0;

        nodes[i].NodeType = AsgNodeType.ForLoop;
        nodes[i].Children.AddRange([nodes[i + 1], nodes[i + 2], nodes[i + 3], nodes[i + 4]]);
        nodes.RemoveRange(i + 1, 4);

        return 0;
    }
}