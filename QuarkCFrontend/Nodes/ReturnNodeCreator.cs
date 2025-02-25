using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;

namespace QuarkCFrontend.Nodes;

public class ReturnNodeCreator : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => AsgNodeType.Return;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (nodes[i].LexemeType != QuarkLexemeType.Return) return 0;

        nodes[i].Children.Add(nodes[i + 1]);
        nodes[i].NodeType = AsgNodeType.Return;
        nodes.RemoveAt(i + 1);
        return 0;
    }
}