using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class OrNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Or, QuarkLexemeType.Or);