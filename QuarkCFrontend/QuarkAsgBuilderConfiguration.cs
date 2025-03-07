namespace QuarkCFrontend;

public static class QuarkAsgBuilderConfiguration
{
    public static AsgBuilderConfiguration<QuarkLexemeType> CreateDefault() => new(
        new SortedDictionary<float, List<INodeCreator<QuarkLexemeType>>>
        {
            {
                1f, [
                    new CommentNodeCreator(),
                    new NumberNodeCreator(),
                    new StringNodeCreator(),
                    new LabelNodeCreator(),
                ]
            },
            {
                2f, [
                    new ScopesNodeCreator(),
                ]
            },
            {
                3f, [
                    new FunctionCallNodeCreator(),
                ]
            },
            {
                4f, [
                    new PowerNodeCreator(),
                ]
            },
            {
                5f, [
                    new MultiplicationNodeCreator(),
                    new DivisionNodeCreator(),
                ]
            },
            {
                6f, [
                    new AdditionNodeCreator(),
                    new SubtractionNodeCreator(),
                    new ModulusNodeCreator(),
                ]
            },
            {
                7f, [
                    new LessThanNodeCreator(),
                    new GreaterThanNodeCreator(),
                    new LessThanOrEqualsNodeCreator(),
                    new GreaterThanOrEqualsNodeCreator(),
                    new EqEqNodeCreator(),
                    new NeqNodeCreator(),
                ]
            },
            {
                8f, [
                    new NotNodeCreator(),
                ]
            },
            {
                9f, [
                    new AndNodeCreator(),
                ]
            },
            {
                10f, [
                    new OrNodeCreator(),
                    new XorNodeCreator(),
                ]
            },
            {
                11f, [
                    new SetVariableNodeCreator(),
                    new BrIfNodeCreator(),
                ]
            },
            {
                12f, [
                    new LoadIdentifierNodeCreator(),
                ]
            },
            {
                13f, [
                    new IfNodeCreator(),
                    new ElifNodeCreator(),
                    new ElseNodeCreator(),
                ]
            },
            {
                14f, [
                    new ForLoopNodeCreator(),
                ]
            },
            {
                15f, [
                    new ImportNodeCreator(),
                    new ReturnNodeCreator(),
                ]
            },
            {
                16f, [
                    new FunctionCreationNodeCreator(),
                ]
            },
        }
    );
}