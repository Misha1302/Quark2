using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class GreaterThanOrEqualsNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.GreaterThanOrEqual, LexemeType.Modulus);