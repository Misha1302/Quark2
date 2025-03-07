using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkStructures;

public class FieldGetNodeCreator : FieldAccessNodeCreator
{
    public static readonly AsgNodeType FieldGet = (AsgNodeType)AsgNodeTypeHelper.GetNextFreeNumber();

    public override AsgNodeType NodeType => FieldGet;


    protected override bool CanBuild(List<AsgNode<QuarkLexemeType>> nodes, int i) =>
        (i + 2 >= nodes.Count || nodes[i + 2].LexemeType != QuarkLexemeType.Eq) &&
        nodes[i].LexemeType == QuarkExtStructures.FieldAccess && nodes[i].NodeType != FieldSetNodeCreator.FieldSet;
}