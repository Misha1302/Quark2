namespace QuarkCFrontend.Asg.Nodes.Interfaces;

public interface INodeCreator
{
    public AsgNodeType NodeType { get; }
    void TryBuild(List<AsgNode> nodes, int i);
}