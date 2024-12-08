using QuarkCFrontend.Asg.Nodes;
using QuarkCFrontend.Asg.Nodes.Interfaces;

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
            new ImportNodeCreator(),
            new ReturnNodeCreator(),
            new FunctionCallNodeCreator(),
        ],
        [
            new FunctionCreationNodeCreator(),
        ],
    ];
}