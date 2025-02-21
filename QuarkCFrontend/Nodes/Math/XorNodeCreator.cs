using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class XorNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Xor, LexemeType.Xor);