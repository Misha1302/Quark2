using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class LessThanOrEqualsNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.LessThanOrEqual, LexemeType.Le);