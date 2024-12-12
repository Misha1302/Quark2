namespace QuarkCFrontend.Asg.Nodes.Interfaces;

public interface INodeCreator
{
    public AsgNodeType NodeType { get; }

    // returns offset for i
    int TryBuild(List<AsgNode> nodes, int i, AsgBuilder asgBuilder)
    {
        if (i < 0 || i >= nodes.Count) return 0;
        if (nodes[i].NodeType == NodeType) return 0;

        return TryBuildImpl(nodes, i, asgBuilder);
    }

    // returns offset for i
    int TryBuildImpl(List<AsgNode> nodes, int i, AsgBuilder asgBuilder);
}