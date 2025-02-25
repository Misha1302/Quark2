using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class XorNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Xor, QuarkLexemeType.Xor);