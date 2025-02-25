namespace DefaultAstImpl.Asg.Interfaces;

public interface INodeCreator<T> where T : struct
{
    public AsgNodeType NodeType { get; }

    // returns offset for i
    int TryBuild(List<AsgNode<T>> nodes, int i)
    {
        if (i < 0 || i >= nodes.Count) return 0;
        if (nodes[i].NodeType == NodeType) return 0;

        return TryBuildImpl(nodes, i);
    }

    // returns offset for i
    int TryBuildImpl(List<AsgNode<T>> nodes, int i);
}