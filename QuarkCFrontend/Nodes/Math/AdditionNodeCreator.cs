using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class AdditionNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Addition, QuarkLexemeType.Addition);