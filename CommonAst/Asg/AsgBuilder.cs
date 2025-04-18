namespace DefaultAstImpl.Asg;

public class AsgBuilder<T>(AsgBuilderConfiguration<T> configuration)
{
    public AsgNode<T> Build(List<LexemeValue<T>> lexemes)
    {
        var nodes = lexemes.Select(x => new AsgNode<T>(AsgNodeType.Unknown, x, [])).ToList();

        var root = new AsgNode<T>(AsgNodeType.Scope, null!, nodes);


        foreach (var level in configuration.CreatorLevels)
        {
            Int128 prevHashCode, curHashCode;
            do
            {
                prevHashCode = nodes.CalcHashCodeForNodes();
                Dfs(nodes, level.Value);
                curHashCode = nodes.CalcHashCodeForNodes();
            } while (curHashCode != prevHashCode);
        }


        return root;
    }

    private void Dfs(List<AsgNode<T>> nodes, IReadOnlyList<INodeCreator<T>> curCreators)
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];

            Dfs(node.Children, curCreators);

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var creator in curCreators)
                if (i < nodes.Count)
                    i += creator.TryBuild(nodes, i);
        }
    }
}