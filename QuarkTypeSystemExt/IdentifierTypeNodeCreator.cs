using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl;

namespace QuarkTypeSystemExt;

public class IdentifierTypeNodeCreator : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType { get; } = (AsgNodeType)AsgNodeTypeHelper.GetNextFreeNumber();

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (i + 2 >= nodes.Count) return 0;
        if (nodes[i].LexemeType != QuarkLexemeType.Identifier
            || nodes[i + 1].LexemeType != QuarkTypeSystemExt.Colon
            || nodes[i + 2].LexemeType != QuarkLexemeType.Identifier
           )
            return 0;

        nodes[i].Children.Add(nodes[i + 1]);
        nodes[i].Children.Add(nodes[i + 2]);
        nodes.RemoveRange(i + 1, 2);
        return 0;
    }
}