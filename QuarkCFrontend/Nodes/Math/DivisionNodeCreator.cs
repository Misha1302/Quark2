using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class DivisionNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Division, LexemeType.Division);