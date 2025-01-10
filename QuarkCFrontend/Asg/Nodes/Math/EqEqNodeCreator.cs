using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class EqEqNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Equal, LexemeType.EqEq);