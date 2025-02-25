using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class AndNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.And, QuarkLexemeType.And);