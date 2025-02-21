using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes;

public class NumberNodeCreator() : ConstantNodeCreatorBase(LexemeType.String, AsgNodeType.String);