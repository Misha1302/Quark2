using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;

namespace QuarkCFrontend.Nodes.Math;

public class ModulusNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Modulus, LexemeType.Modulus);