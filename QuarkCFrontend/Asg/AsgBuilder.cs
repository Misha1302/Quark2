using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg;

public class AsgBuilder(List<List<INodeCreator>> creatorLevels)
{
    public AsgNode Build(List<LexemeValue> lexemes)
    {
        var nodes = lexemes.Select(x => new AsgNode(AsgNodeType.Unknown, x, [])).ToList();

        var root = new AsgNode(AsgNodeType.Scope, null!, nodes);


        foreach (var level in creatorLevels)
        {
            Int128 prevHashCode, curHashCode;
            do
            {
                prevHashCode = nodes.CalcHashCodeForNodes();
                Dfs(nodes, level);
                curHashCode = nodes.CalcHashCodeForNodes();
            } while (curHashCode != prevHashCode);
        }


        return root;
    }

    private void Dfs(List<AsgNode> nodes, IReadOnlyList<INodeCreator> curCreators)
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];

            Dfs(node.Children, curCreators);

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var creator in curCreators)
                if (i < nodes.Count)
                    i += creator.TryBuild(nodes, i, this);
        }
    }
}