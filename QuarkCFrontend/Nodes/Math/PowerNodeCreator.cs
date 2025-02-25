using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class PowerNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Power, QuarkLexemeType.Power);