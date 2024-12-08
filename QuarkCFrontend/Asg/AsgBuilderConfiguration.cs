using QuarkCFrontend.Asg.Nodes;
using QuarkCFrontend.Asg.Nodes.Interfaces;
using QuarkCFrontend.Asg.Nodes.Math;

namespace QuarkCFrontend.Asg;

public static class AsgBuilderConfiguration
{
    public static readonly List<List<INodeCreator>> Default =
    [
        [
            new NumberNodeCreator(),
            new StringNodeCreator(),
        ],
        [
            new ScopesNodeCreator(),
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
            new ImportNodeCreator(),
            new ReturnNodeCreator(),
            new FunctionCallNodeCreator(),
        ],
        [
            new FunctionCreationNodeCreator(),
        ],
    ];
}