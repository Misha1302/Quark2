using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes;

public class CommentNodeCreator : INodeCreator<QuarkLexemeType>
{
    public AsgNodeType NodeType => AsgNodeType.Removed;

    // just remove comment
    public int TryBuildImpl(List<AsgNode<QuarkLexemeType>> nodes, int i)
    {
        if (nodes[i].LexemeType != QuarkLexemeType.Comment) return 0;

        nodes.RemoveAt(i);
        return -1;
    }
}