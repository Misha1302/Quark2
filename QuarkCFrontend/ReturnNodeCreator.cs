namespace QuarkCFrontend;

public class ReturnNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Return;

    public void TryBuild(List<AsgNode> nodes, int i)
    {
        if (nodes[i].LexemeType != LexemeType.Return) return;

        nodes[i].Children.Add(nodes[i + 1]);
        nodes[i].NodeType = AsgNodeType.Return;
        nodes.RemoveAt(i + 1);
    }
}