using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class LessThanNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.LessThan, LexemeType.Lt);