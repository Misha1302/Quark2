using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class MultiplicationNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Multiplication, LexemeType.Multiplication);