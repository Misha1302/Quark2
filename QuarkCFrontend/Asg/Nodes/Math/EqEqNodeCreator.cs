using QuarkCFrontend.Asg.Nodes.Math;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public class EqEqNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Equal, LexemeType.EqEq);