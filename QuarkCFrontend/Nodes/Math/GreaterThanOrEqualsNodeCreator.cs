using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class GreaterThanOrEqualsNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.GreaterThanOrEqual, QuarkLexemeType.Modulus);