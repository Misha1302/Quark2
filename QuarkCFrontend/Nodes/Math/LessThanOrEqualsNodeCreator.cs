using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class LessThanOrEqualsNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.LessThanOrEqual, QuarkLexemeType.Le);