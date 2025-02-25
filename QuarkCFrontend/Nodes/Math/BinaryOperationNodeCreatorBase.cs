namespace QuarkCFrontend.Nodes.Math;

public abstract class BinaryOperationNodeCreatorBase(AsgNodeType nodeType, QuarkLexemeType quarkLexemeType)
    : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => nodeType;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (i + 1 >= nodes.Count) return 0;
        if (nodes[i + 1].Children.Count != 0) return 0;
        if (nodes[i + 1].LexemeType != quarkLexemeType) return 0;

        nodes[i + 1].Children.AddRange([nodes[i], nodes[i + 2]]);
        nodes[i + 1].NodeType = nodeType;

        nodes.RemoveAt(i + 2);
        nodes.RemoveAt(i);
        return -1;
    }
}