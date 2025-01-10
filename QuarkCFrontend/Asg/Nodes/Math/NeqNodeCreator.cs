using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class NeqNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.NotEqual, LexemeType.Neq);