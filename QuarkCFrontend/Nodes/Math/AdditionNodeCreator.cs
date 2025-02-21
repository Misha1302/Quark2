using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class AdditionNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Addition, LexemeType.Addition);