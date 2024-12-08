using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public abstract class SingleOpNodeCreatorBase(AsgNodeType nodeType, LexemeType lexemeType) : INodeCreator
{
    public AsgNodeType NodeType => nodeType;

    public int TryBuild(List<AsgNode> nodes, int i, AsgBuilder asgBuilder)
    {
        if (i + 1 >= nodes.Count) return 0;
        if (nodes[i + 1].Children.Count != 0) return 0;
        if (nodes[i + 1].LexemeType != lexemeType) return 0;

        nodes[i + 1].Children.AddRange([nodes[i]]);
        nodes[i + 1].NodeType = nodeType;

        nodes.RemoveAt(i + 1);
        return 0;
    }
}