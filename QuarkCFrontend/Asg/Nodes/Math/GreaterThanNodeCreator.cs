using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class GreaterThanNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.GreaterThan, LexemeType.Gt);