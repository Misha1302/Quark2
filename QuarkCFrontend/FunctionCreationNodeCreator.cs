using static QuarkCFrontend.LexemeType;

namespace QuarkCFrontend;

public class FunctionCreationNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.FunctionCreating;

    public void TryBuild(List<AsgNode> nodes, int i)
    {
        if (i + 2 >= nodes.Count) return;

        var a = nodes[i].LexemeType == Identifier;
        var b = nodes[i + 1].LexemeType == Identifier;
        var c = nodes[i + 2].NodeType == AsgNodeType.Scope;
        var d = nodes[i + 3].NodeType == AsgNodeType.Scope;
        if (!(a && b && c && d)) return;

        nodes[i + 1].Children.AddRange([nodes[i], nodes[i + 2], nodes[i + 3]]);
        nodes[i + 0].NodeType = AsgNodeType.Type;
        nodes[i + 1].NodeType = AsgNodeType.FunctionCreating;

        nodes.RemoveAt(i + 3);
        nodes.RemoveAt(i + 2);
        nodes.RemoveAt(i);
    }
}