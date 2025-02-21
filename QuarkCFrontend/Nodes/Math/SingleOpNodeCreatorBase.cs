using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public abstract class SingleOpNodeCreatorBase(AsgNodeType nodeType, LexemeType lexemeType) : INodeCreator
{
    public AsgNodeType NodeType => nodeType;

    public int TryBuildImpl(List<AsgNode> nodes, int i)
    {
        if (i + 1 >= nodes.Count) return 0;
        if (nodes[i].Children.Count != 0) return 0;
        if (nodes[i].LexemeType != lexemeType) return 0;

        nodes[i].Children.AddRange([nodes[i + 1]]);
        nodes[i].NodeType = nodeType;

        nodes.RemoveAt(i + 1);
        return 0;
    }
}