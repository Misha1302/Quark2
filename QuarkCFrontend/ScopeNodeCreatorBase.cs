namespace QuarkCFrontend;

public abstract class ScopeNodeCreatorBase
{
    public AsgNodeType NodeType => AsgNodeType.Scope;

    protected void TryBuild(List<AsgNode> nodes, int i, LexemeType left, LexemeType right)
    {
        if (nodes[i].LexemeType != left) return;

        i++;
        var from = i;
        while (true)
        {
            if (nodes[i].LexemeType == left)
                TryBuild(nodes, i, left, right);

            if (nodes[i].LexemeType == right)
                break;

            i++;
        }

        var to = i - 1;

        var node = new AsgNode(AsgNodeType.Scope, null, []);
        node.Children.AddRange(nodes[from..(to + 1)]);

        nodes.RemoveRange(from - 1, to - from + 1 + 2);
        nodes.Insert(from - 1, node);
    }
}