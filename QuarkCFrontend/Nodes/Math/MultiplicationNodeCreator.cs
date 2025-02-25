using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class MultiplicationNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Multiplication, QuarkLexemeType.Multiplication);