using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class DivisionNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Division, QuarkLexemeType.Division);