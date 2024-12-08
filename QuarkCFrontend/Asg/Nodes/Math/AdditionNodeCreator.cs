using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class AdditionNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Addition, LexemeType.Addition);