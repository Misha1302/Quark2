using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class NotNodeCreator() : SingleOpNodeCreatorBase(AsgNodeType.Not, LexemeType.Not);