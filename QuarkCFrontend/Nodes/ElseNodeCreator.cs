namespace QuarkCFrontend.Nodes;

public class ElseNodeCreator : ElseElifNodeCreatorBase
{
    public override AsgNodeType NodeType => AsgNodeType.Else;

    public override int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (i + 1 >= nodes.Count || i <= 0) return 0;
        if (nodes[i - 1].LexemeType != If) return 0;
        if (nodes[i].LexemeType != Else) return 0;

        nodes[i].NodeType = AsgNodeType.Else;
        nodes[i].Children.AddRange([nodes[i + 1]]);
        nodes.RemoveAt(i + 1);

        FindIfToSetAsParent(nodes[i - 1], nodes[i]);
        nodes.RemoveAt(i);

        return -1;
    }
}