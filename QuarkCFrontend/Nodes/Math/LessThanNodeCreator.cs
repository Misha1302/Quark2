using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class LessThanNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.LessThan, LexemeType.Lt);