using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class EqEqNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Equal, QuarkLexemeType.EqEq);