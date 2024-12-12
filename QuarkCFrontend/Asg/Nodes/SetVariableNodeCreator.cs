using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public class SetVariableNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.SetOperation;

    public int TryBuildImpl(List<AsgNode> nodes, int i, AsgBuilder asgBuilder)
    {
        if (i + 1 >= nodes.Count) return 0;
        if (nodes[i + 1].LexemeType != LexemeType.Eq) return 0;
        if (nodes[i + 1].Children.Count != 0) return 0;

        nodes[i + 1].Children.Add(nodes[i]);
        nodes[i + 1].Children.Add(nodes[i + 2]);
        nodes[i + 1].NodeType = AsgNodeType.SetOperation;

        nodes.RemoveAt(i + 2);
        nodes.RemoveAt(i);
        return 0;
    }
}