using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;

namespace QuarkCFrontend.Nodes;

public abstract class ConstantNodeCreatorBase(QuarkLexemeType quarkLexemeType, AsgNodeType nodeType)
    : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => AsgNodeType.Number;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (nodes[i].LexemeType == quarkLexemeType)
            nodes[i].NodeType = nodeType;
        return 0;
    }
}