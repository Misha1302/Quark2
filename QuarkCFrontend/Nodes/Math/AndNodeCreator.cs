using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class AndNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.And, LexemeType.And);