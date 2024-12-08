using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public class FunctionCallNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.FunctionCall;

    public void TryBuild(List<AsgNode> nodes, int i)
    {
        if (i + 1 >= nodes.Count) return;

        var a = nodes[i].LexemeType == LexemeType.Identifier;
        var b = nodes[i + 1].NodeType == AsgNodeType.Scope;
        var c = i + 2 >= nodes.Count || nodes[i + 2].NodeType != AsgNodeType.Scope;
        if (!a || !b || !c) return;

        nodes[i].Children.Add(nodes[i + 1]);
        nodes.RemoveAt(i + 1);
        nodes[i].NodeType = AsgNodeType.FunctionCall;
    }
}