using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class OrNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Or, LexemeType.Or);