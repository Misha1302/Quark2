namespace QuarkCFrontend.Nodes;

public abstract class ElseElifNodeCreatorBase : INodeCreator<QuarkLexemeType>
{
    public abstract AsgNodeType NodeType { get; }
    public abstract int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i);


    protected void FindIfToSetAsParent(AsgNode<QuarkLexemeType> node, AsgNode<QuarkLexemeType> curChild)
    {
        if (node.Children is [_, _, { LexemeType: If or ElseIf }, ..])
        {
            FindIfToSetAsParent(node.Children[2], curChild);
            return;
        }

        node.Children.Add(curChild);
    }
}