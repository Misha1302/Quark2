using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class GreaterThanNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.GreaterThan, QuarkLexemeType.Gt);