using QuarkCFrontend.Asg.Nodes.Math;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg;

public class EqEqNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Equal, LexemeType.EqEq);