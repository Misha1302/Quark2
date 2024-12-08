namespace QuarkCFrontend;

public class ImportNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Import;

    public void TryBuild(List<AsgNode> nodes, int i)
    {
        if (nodes[i].LexemeType != LexemeType.Import) return;

        nodes[i].Children.Add(nodes[i + 1]);
        nodes[i].NodeType = AsgNodeType.Import;
        nodes.RemoveAt(i + 1);
    }
}