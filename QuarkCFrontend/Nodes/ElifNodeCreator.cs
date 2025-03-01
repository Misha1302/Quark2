namespace QuarkCFrontend.Nodes;

public class ElifNodeCreator : ElseElifNodeCreatorBase
{
    public override AsgNodeType NodeType => AsgNodeType.ElseIf;

    public override int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (i + 2 >= nodes.Count || i <= 0) return 0;
        if (nodes[i - 1].LexemeType != If) return 0;
        if (nodes[i].LexemeType != ElseIf) return 0;

        nodes[i].NodeType = AsgNodeType.ElseIf;
        nodes[i].Children.AddRange([nodes[i + 1], nodes[i + 2]]);
        nodes.RemoveAt(i + 2);
        nodes.RemoveAt(i + 1);

        FindIfToSetAsParent(nodes[i - 1], nodes[i]);
        nodes.RemoveAt(i);

        return -1;
    }
}