using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class DivisionNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Division, QuarkLexemeType.Division);