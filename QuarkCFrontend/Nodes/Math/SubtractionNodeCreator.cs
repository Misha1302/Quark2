using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class SubtractionNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Subtraction, QuarkLexemeType.Subtraction);