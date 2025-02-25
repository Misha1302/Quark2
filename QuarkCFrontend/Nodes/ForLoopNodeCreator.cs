namespace QuarkCFrontend.Nodes;

public class ForLoopNodeCreator : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => AsgNodeType.ForLoop;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (i + 4 >= nodes.Count) return 0;
        if (nodes[i].LexemeType != For) return 0;

        nodes[i].NodeType = AsgNodeType.ForLoop;
        nodes[i].Children.AddRange([nodes[i + 1], nodes[i + 2], nodes[i + 3], nodes[i + 4]]);
        nodes.RemoveRange(i + 1, 4);


        return 0;
    }
}