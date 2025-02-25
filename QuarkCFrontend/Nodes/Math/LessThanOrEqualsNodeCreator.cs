using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class LessThanOrEqualsNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.LessThanOrEqual, QuarkLexemeType.Le);