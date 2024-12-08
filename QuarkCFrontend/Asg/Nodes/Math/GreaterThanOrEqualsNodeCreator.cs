using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class GreaterThanOrEqualsNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.GreaterThanOrEqual, LexemeType.Modulus);