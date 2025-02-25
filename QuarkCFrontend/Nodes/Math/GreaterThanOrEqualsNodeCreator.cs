using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class GreaterThanOrEqualsNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.GreaterThanOrEqual, QuarkLexemeType.Modulus);