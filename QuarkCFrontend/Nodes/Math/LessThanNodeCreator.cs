using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class LessThanNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.LessThan, QuarkLexemeType.Lt);