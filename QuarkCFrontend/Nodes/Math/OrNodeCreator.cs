using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class OrNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Or, QuarkLexemeType.Or);