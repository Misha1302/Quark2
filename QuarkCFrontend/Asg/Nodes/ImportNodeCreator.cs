using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public class ImportNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Import;

    public int TryBuild(List<AsgNode> nodes, int i, AsgBuilder asgBuilder)
    {
        if (nodes[i].LexemeType != LexemeType.Import) return 0;

        nodes[i].Children.Add(nodes[i + 1]);
        nodes[i].NodeType = AsgNodeType.Import;
        nodes.RemoveAt(i + 1);
        return 0;
    }
}