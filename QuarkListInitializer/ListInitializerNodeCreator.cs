using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl;

namespace QuarkListInitializer;

public class ListInitializerNodeCreator : INodeCreator<QuarkLexemeType>
{
    public static readonly AsgNodeType ListInitializer = (AsgNodeType)AsgNodeTypeHelper.GetNextFreeNumber();
    public AsgNodeType NodeType => ListInitializer;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (nodes[i].NodeType != AsgNodeType.Scope) return 0;
        if (nodes[i].LexemeValue?.Text != "{") return 0;
        if (nodes.Take(i).Any(x => x.LexemeType.IsNeedBlock())) return 0;

        nodes[i].NodeType = NodeType;
        return 0;
    }
}