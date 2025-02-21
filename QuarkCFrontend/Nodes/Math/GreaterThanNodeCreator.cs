using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class GreaterThanNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.GreaterThan, LexemeType.Gt);