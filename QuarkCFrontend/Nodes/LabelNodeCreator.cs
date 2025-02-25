using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;

namespace QuarkCFrontend.Nodes;

public class LabelNodeCreator : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => AsgNodeType.Label;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (nodes[i].LexemeType == QuarkLexemeType.Label)
            nodes[i].NodeType = AsgNodeType.Label;
        return 0;
    }
}