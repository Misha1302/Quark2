using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class ModulusNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Modulus, QuarkLexemeType.Modulus);