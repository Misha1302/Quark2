using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class LessThanOrEqualsNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.LessThanOrEqual, LexemeType.Le);