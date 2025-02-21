using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class SubtractionNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Subtraction, LexemeType.Subtraction);