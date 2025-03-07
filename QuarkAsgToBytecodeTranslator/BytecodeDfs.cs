namespace AsgToBytecodeTranslator;

public class BytecodeDfs
{
    public void Dfs<T>(AsgNode<T> node, Action<AsgNode<T>> action)
    {
        foreach (var nodeChild in node.Children) Dfs(nodeChild, action);
        action(node);
    }
}