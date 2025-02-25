using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;

namespace QuarkCFrontend.Nodes.Math;

public abstract class SingleOpNodeCreatorBase(AsgNodeType nodeType, QuarkLexemeType quarkLexemeType)
    : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => nodeType;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (i + 1 >= nodes.Count) return 0;
        if (nodes[i].Children.Count != 0) return 0;
        if (nodes[i].LexemeType != quarkLexemeType) return 0;

        nodes[i].Children.AddRange([nodes[i + 1]]);
        nodes[i].NodeType = nodeType;

        nodes.RemoveAt(i + 1);
        return 0;
    }
}