using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class OrNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Or, LexemeType.Or);