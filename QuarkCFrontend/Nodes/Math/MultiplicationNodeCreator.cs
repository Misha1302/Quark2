using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class MultiplicationNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Multiplication, QuarkLexemeType.Multiplication);