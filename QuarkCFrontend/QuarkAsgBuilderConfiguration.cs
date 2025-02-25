using DefaultAstImpl.Asg.Interfaces;
using DefaultLexerImpl;
using QuarkCFrontend.Nodes;
using QuarkCFrontend.Nodes.Math;

namespace QuarkCFrontend;

public static class QuarkAsgBuilderConfiguration
{
    public static List<List<INodeCreator<QuarkLexemeType>>> CreateDefault() =>
    [
        [
            new CommentNodeCreator(),
            new NumberNodeCreator(),
            new StringNodeCreator(),
            new LabelNodeCreator(),
        ],
        [
            new ScopesNodeCreator(),
        ],
        [
            new FunctionCallNodeCreator(),
        ],
        [
            new PowerNodeCreator(),
        ],
        [
            new MultiplicationNodeCreator(),
            new DivisionNodeCreator(),
        ],
        [
            new AdditionNodeCreator(),
            new SubtractionNodeCreator(),
            new ModulusNodeCreator(),
        ],
        [
            new LessThanNodeCreator(),
            new GreaterThanNodeCreator(),
            new LessThanOrEqualsNodeCreator(),
            new GreaterThanOrEqualsNodeCreator(),
            new EqEqNodeCreator(),
            new NeqNodeCreator(),
        ],
        [
            new NotNodeCreator(),
        ],
        [
            new AndNodeCreator(),
        ],
        [
            new OrNodeCreator(),
            new XorNodeCreator(),
        ],
        [
            new SetVariableNodeCreator(),
            new BrIfNodeCreator(),
        ],
        [
            new LoadIdentifierNodeCreator(),
        ],
        [
            new IfNodeCreator(),
        ],
        [
            new ForLoopNodeCreator(),
        ],
        [
            new ImportNodeCreator(),
            new ReturnNodeCreator(),
        ],
        [
            new FunctionCreationNodeCreator(),
        ],
    ];
}