using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class LessThanNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.LessThan, QuarkLexemeType.Lt);