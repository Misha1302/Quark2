using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes;

public class StringNodeCreator() : ConstantNodeCreatorBase(LexemeType.Number, AsgNodeType.Number);