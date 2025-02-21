using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class MultiplicationNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Multiplication, LexemeType.Multiplication);