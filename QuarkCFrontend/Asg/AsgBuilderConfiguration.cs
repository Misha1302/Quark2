using QuarkCFrontend.Asg.Nodes;
using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Asg.Nodes.Math;

namespace QuarkCFrontend.Asg;

public static class AsgBuilderConfiguration
{
    public static readonly List<List<INodeCreator>> Default =
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