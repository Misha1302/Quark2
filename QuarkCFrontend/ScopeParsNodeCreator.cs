namespace QuarkCFrontend;

public class ScopeParsNodeCreator : ScopeNodeCreatorBase, INodeCreator
{
    public void TryBuild(List<AsgNode> nodes, int i)
    {
        base.TryBuild(nodes, i, LexemeType.LeftPar, LexemeType.RightPar);
    }
}