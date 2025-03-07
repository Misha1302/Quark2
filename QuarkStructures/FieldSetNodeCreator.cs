using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkStructures;

public class FieldSetNodeCreator : FieldAccessNodeCreator
{
    public static readonly AsgNodeType FieldSet = (AsgNodeType)AsgNodeTypeHelper.GetNextFreeNumber();

    public override AsgNodeType NodeType => FieldSet;

    protected override bool CanBuild(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (
            nodes[i].LexemeType != QuarkExtStructures.FieldAccess ||
            nodes[i].NodeType == FieldGetNodeCreator.FieldGet
        ) return false;

        var canBuild = false;
        while (i + 2 < nodes.Count)
        {
            if (nodes[i + 2].LexemeType == QuarkLexemeType.Eq)
            {
                canBuild = true;
                break;
            }

            if (nodes[i + 2].LexemeType != QuarkExtStructures.FieldAccess) break;

            i += 2;
        }

        return canBuild;
    }
}