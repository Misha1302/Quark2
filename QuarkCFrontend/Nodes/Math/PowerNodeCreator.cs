using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class PowerNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Power, LexemeType.Power);