using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class PowerNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Power, LexemeType.Power);