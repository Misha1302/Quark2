using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class GreaterThanNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.GreaterThan, QuarkLexemeType.Gt);