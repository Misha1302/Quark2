using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg;

public class AsgBuilder(List<List<INodeCreator>> creatorLevels)
{
    private List<INodeCreator> _curCreators = null!;

    public AsgNode Build(List<LexemeValue> lexemes)
    {
        var nodes = lexemes.Select(x => new AsgNode(AsgNodeType.Unknown, x, [])).ToList();

        var root = new AsgNode(AsgNodeType.Scope, null!, nodes);


        foreach (var level in creatorLevels)
            Dfs(nodes, level);


        return root;
    }

    public void DfsWithMathOps(List<AsgNode> nodes)
    {
        Dfs(nodes, _curCreators);
    }

    private void Dfs(List<AsgNode> nodes, List<INodeCreator> curCreators)
    {
        _curCreators = curCreators;
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];

            Dfs(node.Children, curCreators);

            foreach (var creator in curCreators)
                if (i < nodes.Count)
                    i += creator.TryBuild(nodes, i, this);
        }
    }
}