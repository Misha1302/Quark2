using DefaultAstImpl.Asg;
using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes;

public class ImportNodeCreator : INodeCreator
{
    public AsgNodeType NodeType => AsgNodeType.Import;

    public int TryBuildImpl(List<AsgNode> nodes, int i)
    {
        if (nodes[i].LexemeType != LexemeType.Import) return 0;

        nodes[i].Children.Add(nodes[i + 1]);
        nodes[i].NodeType = AsgNodeType.Import;
        nodes.RemoveAt(i + 1);
        return 0;
    }
}