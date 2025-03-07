using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl;

namespace QuarkStructures;

public class StructNodeCreator : INodeCreator<QuarkLexemeType>
{
    public static readonly AsgNodeType StructType = (AsgNodeType)AsgNodeTypeHelper.GetNextFreeNumber();

    public AsgNodeType NodeType => StructType;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (nodes[i].LexemeType != QuarkExtStructures.StructType) return 0;
        if (i + 2 >= nodes.Count) return 0;

        nodes[i].NodeType = StructType;

        nodes[i].Children.Add(nodes[i + 1]);
        nodes[i].Children.Add(nodes[i + 2]);
        foreach (var child in nodes[i].Children) child.NodeType = StructType;
        nodes.RemoveAt(i + 2);
        nodes.RemoveAt(i + 1);
        return 0;
    }
}