using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes;

public class BrIfNodeCreator : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => AsgNodeType.BrIf;

    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (nodes[i].Children.Count != 0) return 0;
        if (nodes[i].LexemeType != QuarkLexemeType.BrIf) return 0;

        nodes[i].Children.AddRange([nodes[i + 1], nodes[i + 2]]);
        nodes.RemoveAt(i + 2);
        nodes.RemoveAt(i + 1);
        nodes[i].NodeType = AsgNodeType.BrIf;
        return 0;
    }
}