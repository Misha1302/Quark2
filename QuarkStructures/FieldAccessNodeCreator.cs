using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl;

namespace QuarkStructures;

public abstract class FieldAccessNodeCreator : INodeCreator<QuarkLexemeType>
{
    public abstract AsgNodeType NodeType { get; }

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (i + 1 >= nodes.Count || i - 1 < 0) return 0;
        if (!CanBuild(nodes, i)) return 0;

        nodes[i].NodeType = NodeType;

        nodes[i].Children.Add(nodes[i - 1]);
        nodes[i].Children.Add(nodes[i + 1]);
        nodes.RemoveAt(i + 1);
        nodes.RemoveAt(i - 1);

        TryBuildImpl(nodes, i);

        return -1;
    }

    protected abstract bool CanBuild(List<AsgNode<QuarkLexemeType>> nodes, int i);
}