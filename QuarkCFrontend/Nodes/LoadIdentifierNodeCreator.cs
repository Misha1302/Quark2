namespace QuarkCFrontend.Nodes;

public class LoadIdentifierNodeCreator : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => AsgNodeType.Identifier;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (nodes[i].NodeType == AsgNodeType.Unknown && nodes[i].LexemeType == Identifier)
            nodes[i].NodeType = AsgNodeType.Identifier;

        return 0;
    }
}