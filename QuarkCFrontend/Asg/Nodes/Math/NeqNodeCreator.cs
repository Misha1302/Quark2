using QuarkCFrontend.Asg.Nodes.Math;
using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public class NeqNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.NotEqual, LexemeType.Neq);