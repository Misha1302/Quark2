using DefaultAstImpl.Asg;

namespace AsgToBytecodeTranslator;

public class BytecodeDfs
{
    public void Dfs(AsgNode node, Action<AsgNode> action)
    {
        foreach (var nodeChild in node.Children) Dfs(nodeChild, action);
        action(node);
    }
}