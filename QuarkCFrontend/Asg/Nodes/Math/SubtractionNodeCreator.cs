using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class SubtractionNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Subtraction, LexemeType.Subtraction);