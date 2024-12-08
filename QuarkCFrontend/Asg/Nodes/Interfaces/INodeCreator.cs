namespace QuarkCFrontend.Asg.Nodes.Interfaces;

public interface INodeCreator
{
    public AsgNodeType NodeType { get; }

    // returns offset for i
    int TryBuild(List<AsgNode> nodes, int i, AsgBuilder asgBuilder);
}