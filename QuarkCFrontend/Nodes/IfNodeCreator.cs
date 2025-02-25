using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;

namespace QuarkCFrontend.Nodes;

public class IfNodeCreator : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => AsgNodeType.If;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (i + 2 >= nodes.Count) return 0;
        if (nodes[i].LexemeType != QuarkLexemeType.If) return 0;

        nodes[i].NodeType = AsgNodeType.If;
        nodes[i].Children.AddRange([nodes[i + 1], nodes[i + 2]]);
        nodes.RemoveAt(i + 2);
        nodes.RemoveAt(i + 1);

        return 0;
    }
}