using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class EqEqNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Equal, QuarkLexemeType.EqEq);