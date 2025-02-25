using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class NeqNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.NotEqual, QuarkLexemeType.Neq);