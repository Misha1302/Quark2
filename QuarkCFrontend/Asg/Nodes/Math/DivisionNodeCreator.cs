using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class DivisionNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Division, LexemeType.Division);