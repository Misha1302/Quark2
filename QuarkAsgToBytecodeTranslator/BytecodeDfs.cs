using DefaultAstImpl.Asg;

namespace AsgToBytecodeTranslator;

public class BytecodeDfs
{
    public void Dfs<T>(AsgNode<T> node, Action<AsgNode<T>> action) where T : struct
    {
        foreach (var nodeChild in node.Children) Dfs(nodeChild, action);
        action(node);
    }
}