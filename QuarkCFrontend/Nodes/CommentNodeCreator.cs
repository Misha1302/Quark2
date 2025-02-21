using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes;

public class CommentNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Removed;

    // just remove comment
    public int TryBuildImpl(List<AsgNode> nodes, int i)
    {
        if (nodes[i].LexemeType != LexemeType.Comment) return 0;

        nodes.RemoveAt(i);
        return -1;
    }
}