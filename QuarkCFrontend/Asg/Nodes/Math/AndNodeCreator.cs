using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class AndNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.And, LexemeType.And);