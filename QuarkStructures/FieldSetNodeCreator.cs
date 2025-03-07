using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkStructures;

public class FieldSetNodeCreator : FieldAccessNodeCreator
{
    public static readonly AsgNodeType FieldSet = (AsgNodeType)AsgNodeTypeHelper.GetNextFreeNumber();

    public override AsgNodeType NodeType => FieldSet;

    protected override bool CanBuild(List<AsgNode<QuarkLexemeType>> nodes, int i) =>
        i + 2 < nodes.Count && nodes[i + 2].LexemeType == QuarkLexemeTypes.Eq;
}