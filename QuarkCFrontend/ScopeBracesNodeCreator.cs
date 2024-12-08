namespace QuarkCFrontend;

public class ScopeBracesNodeCreator : ScopeNodeCreatorBase, INodeCreator
{
    public void TryBuild(List<AsgNode> nodes, int i)
    {
        base.TryBuild(nodes, i, LexemeType.LeftBrace, LexemeType.RightBrace);
    }
}