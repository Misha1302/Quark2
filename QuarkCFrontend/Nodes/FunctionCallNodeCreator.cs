namespace QuarkCFrontend.Nodes;

public class FunctionCallNodeCreator : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => AsgNodeType.FunctionCall;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (i + 1 >= nodes.Count) return 0;

        var a = nodes[i].LexemeType == Identifier;
        var b = nodes[i + 1].NodeType == AsgNodeType.Scope;
        var c = !(i >= 1 && IsInTheSameLine(nodes[i - 1], nodes[i]) && nodes[i - 1].LexemeType == Def);

        if (!a || !b || !c) return 0;


        nodes[i].Children.Add(nodes[i + 1]);
        nodes.RemoveAt(i + 1);
        nodes[i].NodeType = AsgNodeType.FunctionCall;


        return 0;
    }


    private bool IsInTheSameLine(AsgNode<QuarkLexemeType> a, AsgNode<QuarkLexemeType> b) =>
        a.LineNumber == b.LineNumber;
}